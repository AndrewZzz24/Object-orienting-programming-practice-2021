// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Services;

namespace Banks.Entities
{
    public class DebitAccount : BankAccount
    {
        private const int DaysInYear = 365;

        public DebitAccount(double annualAddingPercent, double operationLimit)
            : base(0, AccountType.DebitAccount, operationLimit)
        {
            DailyAddingPercent = annualAddingPercent / DaysInYear;
        }

        public double DailyAddingPercent
        {
            get;
        }

        public override void CountFee()
        {
            Fee += Money * DailyAddingPercent;
        }

        public override void ChargeFee()
        {
            Money += Fee;
            Money = Math.Round(Money, 3);
        }
    }
}