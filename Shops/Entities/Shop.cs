// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private static int _idCounter;
        private int _money;

        private Dictionary<Product, AmountPrice> _productsInfoList = new Dictionary<Product, AmountPrice>();

        public Shop(string name, string address)
        {
            Name = name;
            Address = address;
            _money = 0;
            Id = ++_idCounter;
        }

        public string Name
        {
            get;
        }

        public string Address
        {
            get;
        }

        public int Id
        {
            get;
        }

        public void AddProduct(Product product)
        {
            DoAddProduct(product, 1);
        }

        public void AddProduct(Product product, int amount)
        {
            DoAddProduct(product, amount);
        }

        public void DeleteProduct(Product product)
        {
            _productsInfoList.Remove(product);
        }

        public void SetProductPrice(Product productToSetPrice, int newPrice)
        {
            if (!_productsInfoList.ContainsKey(productToSetPrice))
                throw new ShopException("No " + productToSetPrice.Name + " product in the shop to set price");

            if (newPrice < 0)
                throw new ShopException("You cannot set price <0");

            _productsInfoList[productToSetPrice].Price = newPrice;
        }

        public int GetProductPrice(Product productToGetPrice)
        {
            CheckProductInShop(productToGetPrice, "No product " + productToGetPrice.Name + " in the shop " + Name);
            return _productsInfoList[productToGetPrice].Price;
        }

        public int GetProductAmount(Product productToFind)
        {
            return _productsInfoList.ContainsKey(productToFind) ? _productsInfoList[productToFind].Amount : 0;
        }

        public void Buy(Customer person, Product productToBuy, int amount)
        {
            CheckProductInShop(
                productToBuy,
                "There is no needed amount of " + productToBuy.Name + " in " + Name + "shop");

            if (_productsInfoList[productToBuy].Amount < amount)
            {
                throw new ShopException("There is no needed amount of " + productToBuy.Name + " in " + Name + "shop");
            }

            if (_productsInfoList[productToBuy].Amount * _productsInfoList[productToBuy].Price > person.Money)
            {
                throw new ShopException("Person has not enough money to buy " + productToBuy.Name + " in amount " +
                                        amount);
            }

            int totalPrice = _productsInfoList[productToBuy].Amount * _productsInfoList[productToBuy].Price;
            _productsInfoList[productToBuy].Amount -= amount;
            person.Money -= totalPrice;
            _money += totalPrice;
        }

        public void BuyShipments(Customer person, List<ProductAmount> shipmentsList)
        {
            int totalPrice = 0;

            foreach (ProductAmount shipment in shipmentsList)
            {
                if (!_productsInfoList.ContainsKey(shipment.Product) ||
                    _productsInfoList[shipment.Product].Amount < shipment.Amount)
                {
                    throw new ShopException("Impossible to buy shipments as shop " + shipment.Product +
                                            " is less than needed");
                }

                totalPrice += _productsInfoList[shipment.Product].Price * shipment.Amount;

                if (totalPrice > person.Money)
                {
                    throw new ShopException(
                        "Impossible to make a purchase as total price is more than customers money :" + totalPrice +
                        " > " +
                        person.Money);
                }
            }

            foreach (ProductAmount shipment in shipmentsList)
            {
                Buy(person, shipment.Product, shipment.Amount);
            }
        }

        private void DoAddProduct(Product productToAdd, int amount)
        {
            if (_productsInfoList.ContainsKey(productToAdd))
            {
                _productsInfoList[productToAdd].Amount += amount;
            }
            else
            {
                if (productToAdd.RecommendedPrice < 0)
                {
                    throw new ShopException("Impossible to add product " + productToAdd.Name + " with Negative price " +
                                            productToAdd.RecommendedPrice);
                }

                _productsInfoList.Add(productToAdd, new AmountPrice(amount, productToAdd.RecommendedPrice));
            }
        }

        private void CheckProductInShop(Product productToCheck, string message)
        {
            if (!_productsInfoList.ContainsKey(productToCheck))
                throw new ShopException(message);
        }
    }
}