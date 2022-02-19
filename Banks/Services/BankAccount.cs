// @author Andrew Zmushko (andrewzmushko@gmail.com)

using Banks.Entities;
using Banks.Tools;

namespace Banks.Services
{
    public abstract class BankAccount
    {
        private const double MinimumAmountValue = 0.0;
        private static int _accountNumber;

        protected BankAccount(double startMoneyAmount, AccountType accountType, double operationLimit)
        {
            CheckAmountCorrectness(startMoneyAmount);
            CheckAmountCorrectness(operationLimit);

            Money = startMoneyAmount;
            AccountType = accountType;
            AccountNumber = ++_accountNumber;
            OperationLimit = operationLimit;
            Fee = MinimumAmountValue;
        }

        public double Money
        {
            get;
            protected set;
        }

        public double Fee
        {
            get;
            protected set;
        }

        public AccountType AccountType
        {
            get;
        }

        public int AccountNumber
        {
            get;
        }

        public double OperationLimit
        {
            get;
        }

        public virtual void AddMoney(double amount)
        {
            CheckAmountCorrectness(amount);
            Money += amount;
        }

        public virtual void WithdrawMoney(double amount)
        {
            CheckAmountCorrectness(amount);
            if (amount > Money)
                throw new BanksException("Impossible to withdraw money amount which is more than account value");
            Money -= amount;
        }

        public abstract void CountFee();

        public virtual void ChargeFee()
        {
        }

        public override string ToString()
        {
            return AccountNumber.ToString();
        }

        private static void CheckAmountCorrectness(double amount)
        {
            if (amount < MinimumAmountValue)
                throw new BanksException("Amount cannot be less than zero");
        }
    }
}