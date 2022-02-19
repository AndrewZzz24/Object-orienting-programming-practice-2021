// @author Andrew Zmushko (andrewzmushko@gmail.com)

namespace Banks.Entities
{
    public class AmountPercent
    {
        public AmountPercent(double amount, double percent)
        {
            Amount = amount;
            Percent = percent;
        }

        public double Amount
        {
            get;
        }

        public double Percent
        {
            get;
        }
    }
}