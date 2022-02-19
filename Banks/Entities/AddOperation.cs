// @author Andrew Zmushko (andrewzmushko@gmail.com)

namespace Banks.Entities
{
    public class AddOperation : IOperation
    {
        public AddOperation(CentralBank centralBank, Bank bank)
        {
            CentralBank = centralBank;
            Bank = bank;
        }

        public CentralBank CentralBank
        {
            get;
        }

        public Bank Bank
        {
            get;
        }

        public void MakeBackOperation(int accountNumber, double amount)
        {
            Bank.WithdrawMoney(accountNumber, amount);
        }
    }
}