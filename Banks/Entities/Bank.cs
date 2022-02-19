// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Services;
using Banks.Tools;

namespace Banks.Entities
{
    public class Bank
    {
        private readonly List<Client> _clientsList = new List<Client>();
        private readonly BankAccountFactory _bankAccountFactory;

        public Bank(CentralBank centralBank, string name, PercentsAndCommissions percentsAndCommissions)
        {
            CentralBank = centralBank ?? throw new BanksException("Central bank cannot be null");
            Name = name ?? throw new BanksException("Name of the bank cannot be null");
            PercentsAndCommissions = percentsAndCommissions ??
                                     throw new BanksException("Percents and commissions cannot be null");
            _bankAccountFactory = new BankAccountFactory(percentsAndCommissions);
        }

        public CentralBank CentralBank
        {
            get;
        }

        public string Name
        {
            get;
        }

        public List<Client> Clients => _clientsList;

        public PercentsAndCommissions PercentsAndCommissions
        {
            get;
        }

        public Bank AddClient(Person person)
        {
            if (person == null)
                throw new BanksException("Person cannot be null");

            _clientsList.Add(new Client(person, new List<BankAccount>()));
            return this;
        }

        public Bank SetPersonAddress(Person person, Address address)
        {
            foreach (Client client in _clientsList.Where(client => client.ClientPersonalInformation == person))
            {
                client.ClientPersonalInformation.Address = address;
                client.CheckSuspectStatus();
                return this;
            }

            throw new BanksException("No such person in the bank");
        }

        public Bank SetPersonPassport(Person person, Passport passport)
        {
            foreach (Client client in _clientsList.Where(client => client.ClientPersonalInformation == person))
            {
                client.ClientPersonalInformation.Passport = passport;
                client.CheckSuspectStatus();
                return this;
            }

            throw new BanksException("No such person in the bank");
        }

        public Bank SetCreditLimit(double limit)
        {
            PercentsAndCommissions.CreditLimit = limit;
            MakeNotification(AccountType.CreditAccount, "Credit limit has been changed! Now it is " + limit);
            return this;
        }

        public Bank SetCreditCommission(int commission)
        {
            PercentsAndCommissions.CreditCommissionIfLimitReached = commission;
            MakeNotification(AccountType.CreditAccount, "Credit commission has been changed! Now it is " + commission);
            return this;
        }

        public Bank SetAnnualAddingPercent(double percent)
        {
            PercentsAndCommissions.AnnualAddingPercent = percent;
            MakeNotification(AccountType.DebitAccount, "Annual adding percent has been changed! Now it is " + percent);
            return this;
        }

        public int CreateAccount(Person person, AccountType accountType)
        {
            foreach (Client client in _clientsList)
            {
                if (client.ClientPersonalInformation.ToString().Equals(person.ToString()))
                {
                    BankAccount account = _bankAccountFactory.CreateBankAccount(client, accountType);
                    client.AddAccount(account);
                    return account.AccountNumber;
                }
            }

            throw new BanksException("There is no person " + person.FullName + " in bank " + Name);
        }

        public int AddMoney(int accountNumber, double amount)
        {
            foreach (Client client in _clientsList)
            {
                foreach (BankAccount bankAccount in client.BankAccounts.Where(bankAccount =>
                    bankAccount.AccountNumber == accountNumber))
                {
                    CheckOperationPossibility(client, amount);
                    bankAccount.AddMoney(amount);

                    var transaction = new Transaction(accountNumber, new AddOperation(CentralBank, this), amount);
                    CentralBank.AddTransaction(transaction);
                    return transaction.TransactionNumber;
                }
            }

            throw new BanksException("No account in the bank " + Name);
        }

        public int WithdrawMoney(int accountNumber, double amount)
        {
            foreach (Client client in _clientsList)
            {
                foreach (BankAccount bankAccount in client.BankAccounts.Where(bankAccount =>
                    bankAccount.AccountNumber == accountNumber))
                {
                    CheckOperationPossibility(client, amount);
                    bankAccount.WithdrawMoney(amount);

                    var transaction = new Transaction(accountNumber, new WithdrawOperation(CentralBank, this), amount);

                    CentralBank.AddTransaction(transaction);
                    return transaction.TransactionNumber;
                }
            }

            throw new BanksException("No Person in the bank " + Name);
        }

        public int TransferMoney(Bank bankTo, int accountNumberFrom, int accountNumberTo, double amount)
        {
            if (Clients.Any(client => client.BankAccounts.Any<BankAccount>(bankAccount => bankAccount.AccountNumber == accountNumberFrom)))
            {
                return CentralBank.TransactionBetweenBank(this, bankTo, accountNumberFrom, accountNumberTo, amount);
            }

            throw new BanksException("No such account in bank " + Name);
        }

        public double GetAccountMoney(int accountNumber)
        {
            foreach (BankAccount bankAccount in Clients.SelectMany(client =>
                client.BankAccounts.Where(bankAccount => bankAccount.AccountNumber == accountNumber)))
            {
                return bankAccount.Money;
            }

            throw new BanksException("No such account in the bank " + Name);
        }

        public void CancelTransaction(int transactionNumber)
        {
            CentralBank.CancelTransaction(transactionNumber);
        }

        public double GetAccountMoney(Person person, int accountNumber)
        {
            foreach (Client client in _clientsList.Where(client => client.ClientPersonalInformation == person))
            {
                foreach (BankAccount bankAccount in client.BankAccounts.Where(bankAccount =>
                    bankAccount.AccountNumber == accountNumber))
                {
                    return bankAccount.Money;
                }

                throw new BanksException("Person" + person.FullName + " does not have a " + accountNumber +
                                         " in bank " + Name);
            }

            throw new BanksException("No " + person.FullName + " in the bank " + Name);
        }

        public void CountFee()
        {
            foreach (BankAccount account in _clientsList.SelectMany(client => client.BankAccounts))
            {
                account.CountFee();
            }
        }

        public void ChargeFee()
        {
            foreach (BankAccount account in _clientsList.SelectMany(client => client.BankAccounts))
            {
                account.ChargeFee();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        private void MakeNotification(AccountType accountType, string message)
        {
            foreach (Client client in from client in Clients
                where client.SubscribeForNotifications
                from account in client.BankAccounts.Where(account => account.AccountType == accountType)
                select client)
            {
                client.Notifications.Add(new Message(message));
            }
        }

        private void CheckOperationPossibility(Client client, double amount)
        {
            if (client.IsSuspected && amount > PercentsAndCommissions.SuspectOperationLimit)
                throw new BanksException();
        }
    }
}