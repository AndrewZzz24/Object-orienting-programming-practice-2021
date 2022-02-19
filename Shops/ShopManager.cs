// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Shops.Entities;
using Shops.Services;
using Shops.Tools;

namespace Shops
{
    public class ShopManager : IShopManager
    {
        private List<Shop> shopsList = new List<Shop>();

        public Shop CreateShop(string name, string address)
        {
            var newShop = new Shop(name, address);
            shopsList.Add(newShop);
            return newShop;
        }

        public Shop GetShop(int id)
        {
            return DoFindShop(null, id);
        }

        public Shop GetShop(string name)
        {
            return DoFindShop(name, -1);
        }

        public Product RegisterProduct(string productName, int price)
        {
            return new Product(productName, price);
        }

        public Shop GetShopWithLowestProductPrice(Product productToBuy, int amount)
        {
            int minPrice = int.MaxValue;
            Shop shopWithLowestPrice = null;

            foreach (Shop shop in shopsList)
            {
                if (shop.GetProductAmount(productToBuy) >= amount)
                {
                    int price = shop.GetProductPrice(productToBuy);

                    if (price < minPrice)
                    {
                        minPrice = price;
                        shopWithLowestPrice = shop;
                    }
                }
            }

            return shopWithLowestPrice;
        }

        private Shop DoFindShop(string name, int id)
        {
            foreach (Shop shop in shopsList)
            {
                if (name != null && shop.Name.Equals(name))
                    return shop;
                if (name == null && shop.Id.Equals(id))
                    return shop;
            }

            if (name != null)
                throw new ShopException("No shop with " + name + " name");

            throw new ShopException("No shop  with " + id + " id");
        }
    }
}