// @author Andrew Zmushko (andrewzmushko@gmail.com)

namespace Banks.Entities
{
    public class Transaction
    {
        private static int _transactionCounter;

        public Transaction(
            int accountNumber,
            IOperation operationType,
            double amount)
        {
            AccountNumber = accountNumber;
            OperationType = operationType;

            TransactionNumber = _transactionCounter++;

            Amount = amount;
        }

        protected Transaction()
        {
        }

        public int AccountNumber
        {
            get;
        }

        public double Amount
        {
            get;
        }

        public int TransactionNumber
        {
            get;
        }

        public IOperation OperationType
        {
            get;
            internal set;
        }
    }
}