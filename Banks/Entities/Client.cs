// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Banks.Services;
using Banks.Tools;

namespace Banks.Entities
{
    public class Client
    {
        private readonly List<Message> _messages = new List<Message>();
        private List<BankAccount> _bankAccounts = new List<BankAccount>();

        public Client(Person clientPersonalInformation, List<BankAccount> bankAccounts)
        {
            ClientPersonalInformation = clientPersonalInformation;
            BankAccounts = bankAccounts;
            if (ClientPersonalInformation.Passport == null || ClientPersonalInformation.Address == null)
                IsSuspected = true;
            else IsSuspected = false;
            SubscribeForNotifications = false;
        }

        public Person ClientPersonalInformation
        {
            get;
        }

        public List<BankAccount> BankAccounts
        {
            get => _bankAccounts;
            private init => _bankAccounts = value;
        }

        public bool IsSuspected
        {
            get;
            private set;
        }

        public List<Message> Notifications => _messages;

        public bool SubscribeForNotifications
        {
            get;
        }

        public void AddAccount(BankAccount bankAccount)
        {
            if (bankAccount == null)
                throw new BanksException("bank account cannot be null");
            BankAccounts.Add(bankAccount);
        }

        public override string ToString()
        {
            return ClientPersonalInformation.ToString();
        }

        public void CheckSuspectStatus()
        {
            if (ClientPersonalInformation.FullName != null
                && ClientPersonalInformation.Address != null
                && ClientPersonalInformation.Passport != null)
                IsSuspected = false;
        }
    }
}