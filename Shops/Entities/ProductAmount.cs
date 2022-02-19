// @author Andrew Zmushko (andrewzmushko@gmail.com)

namespace Shops.Entities
{
    public class ProductAmount
    {
        public ProductAmount(Product product, int amount)
        {
            Amount = amount;
            Product = product;
        }

        public Product Product
        {
            get;
        }

        public int Amount
        {
            get;
        }
    }
}