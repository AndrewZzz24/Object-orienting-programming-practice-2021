// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Services;

namespace Banks.Entities
{
    public class CreditAccount : BankAccount
    {
        public CreditAccount(double limit, double commissionIfLimitReached, double operationLimit)
            : base(0, AccountType.CreditAccount, operationLimit)
        {
            Limit = limit;
            Money = limit;
            CommissionIfLimitReached = commissionIfLimitReached;
        }

        public double Limit
        {
            get;
        }

        public double CommissionIfLimitReached
        {
            get;
        }

        public override void WithdrawMoney(double amount)
        {
            Money -= amount;
        }

        public override void CountFee()
        {
            if (Money < 0)
                Fee += CommissionIfLimitReached;
        }

        public override void ChargeFee()
        {
            Money -= Fee;
            Money = Math.Round(Money, 3);
        }
    }
}