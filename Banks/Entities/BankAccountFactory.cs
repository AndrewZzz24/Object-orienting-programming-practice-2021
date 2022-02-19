// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Services;
using Banks.Tools;

namespace Banks.Entities
{
    public class BankAccountFactory
    {
        public BankAccountFactory(PercentsAndCommissions percentsAndCommissions)
        {
            PercentsAndCommissions = percentsAndCommissions ??
                                     throw new BanksException("Percents and commissions class cannot be null");
        }

        public PercentsAndCommissions PercentsAndCommissions
        {
            get;
        }

        public BankAccount CreateBankAccount(Client client, AccountType accountType)
        {
            double operationLimit = 0;
            if (client.IsSuspected)
            {
                operationLimit = PercentsAndCommissions.SuspectOperationLimit;
            }

            BankAccount account = accountType switch
            {
                AccountType.CreditAccount => new CreditAccount(
                    PercentsAndCommissions.CreditLimit,
                    PercentsAndCommissions.CreditCommissionIfLimitReached,
                    operationLimit),

                AccountType.DebitAccount => new DebitAccount(
                    PercentsAndCommissions.AnnualAddingPercent,
                    operationLimit),

                AccountType.DepositAccount => new DepositAccount(
                    PercentsAndCommissions.DaysForDepositAccount,
                    PercentsAndCommissions.DepositInterestRate,
                    operationLimit),

                _ => throw new BanksException("Invalid account type")
            };

            return account;
        }
    }
}