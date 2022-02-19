// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Entities;
using Banks.Services;
using Spectre.Console;

namespace Banks.Tools
{
    public class MainUi
    {
        private CentralBank _centralBank = null;
        private AccountType[] _accountTypes;

        public void Main1()
        {
            _accountTypes = Enum.GetValues<AccountType>();
            AnsiConsole.Clear();
            GetCommand();
        }

        public void GetCommand()
        {
            while (true)
            {
                string command = CommandController.GetCommand();
                switch (command)
                {
                    case "Create Central Bank":
                        CreateCentralBank();
                        break;
                    case "Create Bank":
                        CreateBank();
                        break;
                    case "Get Banks List":
                        GetBanksList();
                        break;
                    case "Add client":
                        AddClient();
                        break;
                    case "Create Account":
                        CreateAccount();
                        break;
                    case "Add money":
                        AddMoney();
                        break;
                    case "Withdraw money":
                        WithdrawMoney();
                        break;
                    case "Exit":
                        return;
                }
            }
        }

        private void GetBanksList()
        {
            foreach (Bank bank in _centralBank.Banks)
            {
                AnsiConsole.WriteLine(bank.Name);
            }
        }

        private void CreateCentralBank()
        {
            if (_centralBank != null)
                throw new BanksException("Central bank has been already created");
            string name = CommandController.GetBankName();
            _centralBank = new CentralBank(name);
        }

        private void CreateBank()
        {
            if (_centralBank == null)
                throw new BanksException("Impossible to make a bank without central bank");

            string name = CommandController.GetBankName();
            _centralBank.AddBank(name, CommandController.GetPercentsAndCommissions());
        }

        private void AddClient()
        {
            if (_centralBank == null)
                throw new BanksException();

            Bank bank = CommandController.GetBank(_centralBank);
            Person person = CommandController.GetPerson();
            bank.AddClient(person);
        }

        private void CreateAccount()
        {
            Bank bank = CommandController.GetBank(_centralBank);
            Client client = CommandController.GetClient(bank);
            AccountType accountType = CommandController.GetAccountType(_accountTypes);
            bank.CreateAccount(client.ClientPersonalInformation, accountType);
        }

        private void WithdrawMoney()
        {
            MakeTransaction(OperationType.WithdrawMoney);
        }

        private void AddMoney()
        {
            MakeTransaction(OperationType.AddMoney);
        }

        private void MakeTransaction(OperationType operationType)
        {
            Bank bank = CommandController.GetBank(_centralBank);
            Client client = CommandController.GetClient(bank);
            double amount = CommandController.GetAmount();
            BankAccount bankAccount = CommandController.GetBankAccount(client);
            switch (operationType)
            {
                case OperationType.AddMoney:
                    bank.AddMoney(bankAccount.AccountNumber, amount);
                    break;
                case OperationType.WithdrawMoney:
                    bank.WithdrawMoney(bankAccount.AccountNumber, amount);
                    break;
                default:
                    throw new BanksException("Unknown operation type");
            }
        }
    }
}