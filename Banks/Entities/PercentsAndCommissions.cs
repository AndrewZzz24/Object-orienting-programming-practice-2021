// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class PercentsAndCommissions
    {
        private const int LowestPossibleCommission = 0;
        private const int LowestPossibleLimit = 0;
        private const int LowestPossibleAddingPercent = 0;
        private const int LowestSuspectOperationLimit = 0;
        private const int LowestDaysForDepositAccount = 1;

        private int _creditCommissionIfLimitReached;
        private double _annualAddingPercent;
        private DepositInterestRate _depositInterestRate;
        private int _daysForDepositAccount;
        private double _creditLimit;
        private double _suspectOperationLimit;

        public PercentsAndCommissions(
            double annualAddingPercent,
            DepositInterestRate depositInterestRate,
            int creditLimit,
            int creditCommissionIfLimitReached,
            int suspectOperationLimit,
            int daysForDepositAccount)
        {
            AnnualAddingPercent = annualAddingPercent;
            DepositInterestRate = depositInterestRate;
            CreditLimit = creditLimit;
            CreditCommissionIfLimitReached = creditCommissionIfLimitReached;
            SuspectOperationLimit = suspectOperationLimit;
            DaysForDepositAccount = daysForDepositAccount;
        }

        public int CreditCommissionIfLimitReached
        {
            get => _creditCommissionIfLimitReached;
            set
            {
                if (value < LowestPossibleCommission)
                    throw new BanksException();
                _creditCommissionIfLimitReached = value;
            }
        }

        public double AnnualAddingPercent
        {
            get => _annualAddingPercent;
            set
            {
                if (value < LowestPossibleAddingPercent)
                    throw new BanksException();
                _annualAddingPercent = value;
            }
        }

        public DepositInterestRate DepositInterestRate
        {
            get => _depositInterestRate;
            set => _depositInterestRate = value ?? throw new BanksException("Impossible to set deposit interest rate as the value cannot be null");
        }

        public int DaysForDepositAccount
        {
            get => _daysForDepositAccount;
            set
            {
                if (value < LowestDaysForDepositAccount)
                    throw new BanksException("Invalid number of days for deposit account");
                _daysForDepositAccount = value;
            }
        }

        public double CreditLimit
        {
            get => _creditLimit;
            set
            {
                if (value < LowestPossibleLimit)
                    throw new BanksException();
                _creditLimit = value;
            }
        }

        public double SuspectOperationLimit
        {
            get => _suspectOperationLimit;
            set
            {
                if (value < LowestSuspectOperationLimit)
                    throw new BanksException();
                _suspectOperationLimit = value;
            }
        }
    }
}