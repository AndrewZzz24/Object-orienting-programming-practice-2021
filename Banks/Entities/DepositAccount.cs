// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Services;
using Banks.Tools;

namespace Banks.Entities
{
    public class DepositAccount : BankAccount
    {
        private const double StartMoneyDepositAmount = 0.0;

        public DepositAccount(int depositAccountDays, DepositInterestRate depositInterestRate, double operationLimit)
            : base(StartMoneyDepositAmount, AccountType.DepositAccount, operationLimit)
        {
            DaysBeforeEnd = depositAccountDays;
            AccountPercents = depositInterestRate ?? throw new BanksException("Deposit interest rate cannot be null");
        }

        public DepositInterestRate AccountPercents
        {
            get;
        }

        public double IncreasingPercent
        {
            get;
            private set;
        }

        public int DaysBeforeEnd
        {
            get;
            internal set;
        }

        public override void AddMoney(double amount)
        {
            if (Money == StartMoneyDepositAmount)
                GetInterestRate(amount);
            base.AddMoney(amount);
        }

        public override void WithdrawMoney(double amount)
        {
            if (DaysBeforeEnd != 0)
                throw new BanksException("Impossible to withdraw from deposit account as the end date is not reached");
            base.WithdrawMoney(amount);
        }

        public override void CountFee()
        {
            Fee += Money * (IncreasingPercent / 100);
        }

        public override void ChargeFee()
        {
            Money += Fee;
            Money = Math.Round(Money, 3);
        }

        private void GetInterestRate(double amount)
        {
            foreach (AmountPercent type in AccountPercents.InterestRates)
            {
                if (amount <= type.Amount)
                {
                    IncreasingPercent = type.Percent;
                    return;
                }
            }
        }
    }
}