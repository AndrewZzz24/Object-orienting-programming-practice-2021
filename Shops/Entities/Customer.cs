// @author Andrew Zmushko (andrewzmushko@gmail.com)

using Shops.Tools;

namespace Shops.Entities
{
    public class Customer
    {
        private const int NoMoneyAmount = 0;
        private const int MinimalMoneyAmount = 0;
        private int money;

        public Customer(string name, int money)
        {
            Name = name;
            Money = money;
        }

        public Customer(string name)
        {
            Name = name;
            Money = NoMoneyAmount;
        }

        public string Name
        {
            get;
        }

        public int Money
        {
            get
            {
                return money;
            }

            set
            {
                if (value < MinimalMoneyAmount)
                    throw new ShopException("You cannot set negative money amount");
                money = value;
            }
        }
    }
}