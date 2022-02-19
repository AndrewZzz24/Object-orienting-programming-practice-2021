// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using System.Linq;
using Banks.Tools;

namespace Banks.Entities
{
    public class CentralBank
    {
        private readonly List<Bank> _banks = new List<Bank>();
        private readonly List<Transaction> _transactions = new List<Transaction>();

        public CentralBank(string name)
        {
            Name = name ?? throw new BanksException("Name of the central bank cannot be null");
        }

        public string Name
        {
            get;
        }

        public List<Bank> Banks => _banks;

        public List<Transaction> Transactions => _transactions;

        public Bank AddBank(string bankName, PercentsAndCommissions percentsAndCommissions)
        {
            var bank = new Bank(this, bankName, percentsAndCommissions);
            _banks.Add(bank);
            return bank;
        }

        public void CountFee()
        {
            foreach (Bank bank in _banks)
            {
                bank.CountFee();
            }
        }

        public void ChargeFee()
        {
            foreach (Bank bank in _banks)
            {
                bank.ChargeFee();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public void CancelTransaction(int transactionNumber)
        {
            foreach (Transaction transaction in _transactions.Where(transaction => transaction.TransactionNumber == transactionNumber))
            {
                if (transaction.GetType() == typeof(TransactionBetweenBanks))
                {
                    var transactionBetweenBanks = (TransactionBetweenBanks)transaction;
                    CancelTransaction(transactionBetweenBanks.TransactionFrom.TransactionNumber);
                    CancelTransaction(transactionBetweenBanks.TransactionTo.TransactionNumber);
                }
                else
                {
                    transaction.OperationType.MakeBackOperation(transaction.AccountNumber, transaction.Amount);
                }

                Transactions.Remove(transaction);
                return;
            }
        }

        internal int TransactionBetweenBank(Bank bankFrom, Bank bankTo, int accountNumberFrom, int accountNumberTo, double amount)
        {
            bankFrom.WithdrawMoney(accountNumberFrom, amount);
            bankTo.AddMoney(accountNumberTo, amount);
            var transactionFrom =
                new Transaction(accountNumberFrom, new WithdrawOperation(this, bankFrom), amount);
            var transactionTo =
                new Transaction(accountNumberTo, new WithdrawOperation(this, bankTo), amount);
            var transaction = new TransactionBetweenBanks(transactionFrom, transactionTo);

            Transactions.Add(transaction);
            return transaction.TransactionNumber;
        }

        internal void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        internal void RemoveTransaction(Transaction transaction)
        {
            Transactions.Remove(transaction);
        }
    }
}