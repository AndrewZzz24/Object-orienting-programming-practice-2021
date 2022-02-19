// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Banks.Entities;
using Banks.Services;
using Spectre.Console;

namespace Banks.Tools
{
    public static class CommandController
    {
        public static string GetCommand()
        {
            string command = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(14)
                    .AddChoices(MainMenuCommands.MenuCommands));

            return command;
        }

        public static string GetBankName()
        {
            string name = AnsiConsole.Ask<string>("Name of bank the bank: ");
            return name;
        }

        public static Bank GetBank(CentralBank centralBank)
        {
            Bank bank = AnsiConsole.Prompt(
                new SelectionPrompt<Bank>()
                    .PageSize(14)
                    .AddChoices(centralBank.Banks));
            return bank;
        }

        public static Client GetClient(Bank bank)
        {
            Client client = AnsiConsole.Prompt(
                new SelectionPrompt<Client>()
                    .PageSize(14)
                    .AddChoices(bank.Clients));
            return client;
        }

        public static AccountType GetAccountType(AccountType[] accountTypes)
        {
            AccountType accountType = AnsiConsole.Prompt(
                new SelectionPrompt<AccountType>()
                    .PageSize(14)
                    .AddChoices(accountTypes));
            return accountType;
        }

        public static Person GetPerson()
        {
            string surname = AnsiConsole.Ask<string>("Enter surname: ");
            string name = AnsiConsole.Ask<string>("Enter name: ");
            string patronymic = AnsiConsole.Ask<string>("Enter patronymic: ");
            string address = AnsiConsole.Ask<string>("Enter address: ");
            string passport = AnsiConsole.Ask<string>("Enter passport: ");

            return new Person(
                new NameSurnamePatronymic(surname, name, patronymic),
                new Address(address),
                new Passport(passport));
        }

        public static PercentsAndCommissions GetPercentsAndCommissions()
        {
            double annualAddingPercent = AnsiConsole.Ask<double>("Enter bank annual adding percent: ");
            int creditLimit = AnsiConsole.Ask<int>("Enter bank credit limit: ");
            int creditCommissionIfLimitReached =
                AnsiConsole.Ask<int>("Enter credit commission if limit of credit account reached: ");
            int suspectOperationLimit = AnsiConsole.Ask<int>("Enter operation limit for suspect accounts: ");
            int daysForDepositAccount = AnsiConsole.Ask<int>("Enter number of days for deposit accounts: ");

            return new PercentsAndCommissions(
                annualAddingPercent,
                GetDepositInterestRate(),
                creditLimit,
                creditCommissionIfLimitReached,
                suspectOperationLimit,
                daysForDepositAccount);
        }

        public static DepositInterestRate GetDepositInterestRate()
        {
            var depositInterestRates = new List<AmountPercent>();
            AnsiConsole.WriteLine(
                "Entre bank deposit ranges for percent increasing. enter 'infinity' if the range is over");

            while (true)
            {
                string amount = AnsiConsole.Ask<string>("Enter amount: ");
                int percent = AnsiConsole.Ask<int>("Enter percent: ");

                if (amount.Equals("infinity"))
                {
                    depositInterestRates.Add(new AmountPercent(int.MaxValue, percent));
                    break;
                }

                if (!int.TryParse(amount, out int parsedAmount))
                {
                    AnsiConsole.WriteLine("Invalid input");
                    continue;
                }

                depositInterestRates.Add(new AmountPercent(parsedAmount, percent));
            }

            return new DepositInterestRate(depositInterestRates);
        }

        public static double GetAmount()
        {
            double amount = AnsiConsole.Ask<double>("Enter amount: ");
            return amount;
        }

        public static BankAccount GetBankAccount(Client client)
        {
            BankAccount bankAccount = AnsiConsole.Prompt(
                new SelectionPrompt<BankAccount>()
                    .PageSize(14)
                    .AddChoices(client.BankAccounts));
            return bankAccount;
        }
    }
}