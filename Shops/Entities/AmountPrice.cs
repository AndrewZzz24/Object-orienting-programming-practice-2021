// @author Andrew Zmushko (andrewzmushko@gmail.com)

namespace Shops.Entities
{
    public class AmountPrice
    {
        public AmountPrice(int amount, int price)
        {
            Amount = amount;
            Price = price;
        }

        public int Amount
        {
            get;
            set;
        }

        public int Price
        {
            get;
            set;
        }
    }
}