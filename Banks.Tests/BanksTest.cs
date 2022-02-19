// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Banks.Entities;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        [Test]
        public void BasicMoneyOperationsTests()
        {
            var cb = new CentralBank("Russian Central Bank");

            var person1 = new Person(
                new NameSurnamePatronymic("Zmushko", "Andrew", "Sergeevich"),
                new Address("Kronverskiy st"),
                new Passport("6060999333")
            );

            var person2 = new Person(
                new NameSurnamePatronymic("Gromyako", "Vitaliy", "Olegovich"),
                new Address("Venice"),
                new Passport("60609923334")
            );

            var amountPercents = new List<AmountPercent>();
            amountPercents.Add(new AmountPercent(50_000, 3));
            amountPercents.Add(new AmountPercent(100_000, 3.5));
            amountPercents.Add(new AmountPercent(int.MaxValue, 4));

            Bank sber = cb.AddBank("Sberbank", new PercentsAndCommissions(
                3,
                new DepositInterestRate(amountPercents),
                300_000,
                1000,
                5000,
                30
            ));

            sber.AddClient(person1);

            int account1Num = sber.CreateAccount(person1, AccountType.DebitAccount);
            Assert.AreEqual(1, account1Num);

            sber.AddMoney(account1Num, 10_000);
            Assert.AreEqual(10_000, sber.GetAccountMoney(person1, account1Num));


            int account2Num = sber.CreateAccount(person1, AccountType.DepositAccount);
            Assert.AreEqual(2, account2Num);

            Assert.Catch<BanksException>(() =>
                {
                    sber.CreateAccount(person2, AccountType.DepositAccount);
                }
            );

            sber.AddClient(person2);
            int account12Person = sber.CreateAccount(person2, AccountType.DebitAccount);
            int account22Person = sber.CreateAccount(person2, AccountType.CreditAccount);
            int account32Person = sber.CreateAccount(person2, AccountType.DepositAccount);

            sber.AddMoney(account12Person, 99_000);
            sber.AddMoney(account22Person, 99_000);
            sber.AddMoney(account32Person, 99_000);
            Assert.AreEqual(99_000, sber.GetAccountMoney(account12Person));
            Assert.AreEqual(399_000, sber.GetAccountMoney(account22Person));
            Assert.AreEqual(99_000, sber.GetAccountMoney(account32Person));

            int transactionNum1 = sber.WithdrawMoney(account12Person, 99_000);
            int transactionNum2 = sber.WithdrawMoney(account22Person, 99_000);
            Assert.Catch<BanksException>(() =>
            {
                sber.WithdrawMoney(account32Person, 99_000); //deposit date hasn't ended.
            });
            Assert.AreEqual(0, sber.GetAccountMoney(account12Person));
            Assert.AreEqual(300_000, sber.GetAccountMoney(account22Person));
            Assert.AreEqual(99_000, sber.GetAccountMoney(account32Person));

            sber.CancelTransaction(transactionNum1);
            sber.CancelTransaction(transactionNum2);
            Assert.AreEqual(99_000, sber.GetAccountMoney(account12Person));
            Assert.AreEqual(399_000, sber.GetAccountMoney(account22Person));
        }

        [Test]
        public void CreateAccountsAndAddMoney_CheckAccountsMoneySomeTimeLater()
        {
            var cb = new CentralBank("Russian central bank");
            var timeLine = new CentralBankTimeLine(cb);
            Person person1 = Person.Builder(new NameSurnamePatronymic("Aloha", "Dance", "Morphlingovich"))
                .SetAddress(new Address("Kharkiv, Ukraine")).SetPassport(new Passport("332228")).GetPerson();

            var amountPercents = new List<AmountPercent>();
            amountPercents.Add(new AmountPercent(50_000, 3));
            amountPercents.Add(new AmountPercent(100_000, 3.5));
            amountPercents.Add(new AmountPercent(int.MaxValue, 4));

            Bank sber = cb.AddBank("Sberbank", new PercentsAndCommissions(
                3,
                new DepositInterestRate(amountPercents),
                300_000,
                1000,
                5000,
                30
            ));

            sber.AddClient(person1);

            int debitAccountNum = sber.CreateAccount(person1, AccountType.DebitAccount);
            int creditAccountNum = sber.CreateAccount(person1, AccountType.CreditAccount);
            int depositAccountNum = sber.CreateAccount(person1, AccountType.DepositAccount);
            sber.AddMoney(debitAccountNum, 10_000);
            sber.AddMoney(creditAccountNum, 10_000);
            sber.AddMoney(depositAccountNum, 10_000);
            timeLine.NextDay();
            Assert.AreEqual(10_000, sber.GetAccountMoney(debitAccountNum));
            Assert.AreEqual(310_000, sber.GetAccountMoney(creditAccountNum));
            Assert.AreEqual(10_000, sber.GetAccountMoney(depositAccountNum));
            timeLine.NextMonth();
            Assert.AreEqual(12_383.562, sber.GetAccountMoney(debitAccountNum));
            Assert.AreEqual(310_000, sber.GetAccountMoney(creditAccountNum));
            Assert.AreEqual(18_700, sber.GetAccountMoney(depositAccountNum));
        }

        [Test]
        public void TransferMoneyBetweenBanksCheck()
        {
            var cb = new CentralBank("Russian central bank");
            var amountPercents = new List<AmountPercent>();
            amountPercents.Add(new AmountPercent(50_000, 3));
            amountPercents.Add(new AmountPercent(100_000, 3.5));
            amountPercents.Add(new AmountPercent(int.MaxValue, 4));
            Bank sber = cb.AddBank("Sberbank", new PercentsAndCommissions(
                3,
                new DepositInterestRate(amountPercents),
                300_000,
                1000,
                5000,
                30
            ));
            Bank tink = cb.AddBank("Tinkoff", new PercentsAndCommissions(
                2,
                new DepositInterestRate(amountPercents),
                200_000,
                3000,
                7000,
                31
            ));
            Person person1 = Person.Builder(new NameSurnamePatronymic("Aloha", "Dance", "Morphlingovich"))
                .SetAddress(new Address("Kharkiv, Ukraine")).SetPassport(new Passport("332228")).GetPerson();
            Person person2 = Person.Builder(new NameSurnamePatronymic("Gromyako", "Vitaliy", "Olegovich"))
                .SetAddress(new Address("Venice, Italy")).SetPassport(new Passport("332222")).GetPerson();
            sber.AddClient(person1);
            tink.AddClient(person2);
            int sberAccount = sber.CreateAccount(person1, AccountType.DebitAccount);
            sber.AddMoney(sberAccount, 10_000);
            int tinkAccount = tink.CreateAccount(person2, AccountType.DebitAccount);
            sber.TransferMoney(tink, sberAccount, tinkAccount, 1000);
            Assert.AreEqual(9_000, sber.GetAccountMoney(sberAccount));
            Assert.AreEqual(1_000, tink.GetAccountMoney(tinkAccount));
        }
    }
}