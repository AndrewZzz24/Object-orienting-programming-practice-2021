// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;

namespace Shops.Entities
{
    public class Product
    {
        private const int UnknownMoneyAmount = -1;

        public Product(string name, int price)
        {
            RecommendedPrice = price;
            Name = name;
        }

        public Product(string name)
        {
            RecommendedPrice = UnknownMoneyAmount;
            Name = name;
        }

        public string Name
        {
            get;
        }

        public int RecommendedPrice
        {
            get;
            set;
        }
    }
}