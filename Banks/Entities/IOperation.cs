// @author Andrew Zmushko (andrewzmushko@gmail.com)

namespace Banks.Entities
{
    public interface IOperation
    {
        CentralBank CentralBank
        {
            get;
        }

        Bank Bank
        {
            get;
        }

        void MakeBackOperation(int accountNumber, double amount);
    }
}