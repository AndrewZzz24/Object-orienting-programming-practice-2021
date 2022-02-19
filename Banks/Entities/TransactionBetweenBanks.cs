// @author Andrew Zmushko (andrewzmushko@gmail.com)

using Banks.Tools;

namespace Banks.Entities
{
    public class TransactionBetweenBanks : Transaction
    {
        public TransactionBetweenBanks(Transaction transactionFrom, Transaction transactionTo)
        {
            TransactionFrom = transactionFrom ?? throw new BanksException("TransactionFrom cannot be null");
            TransactionTo = transactionTo ?? throw new BanksException("TransactionTo cannot be null");
        }

        public Transaction TransactionFrom
        {
            get;
        }

        public Transaction TransactionTo
        {
            get;
        }
    }
}