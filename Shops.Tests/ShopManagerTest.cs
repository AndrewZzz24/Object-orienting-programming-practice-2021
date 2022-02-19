// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Shops.Tools;
using NUnit.Framework;
using Shops.Entities;

namespace Shops.Tests
{
    public class Tests
    {
        [Test]
        public void AddProductsToShopAndBuy_ProductsHasBeenAddedAndBought()
        {
            var shop1 = new Shop("Diksi", "Nevskiy Prospect");
            var shop2 = new Shop("Pyaterochka", "Kronverskiy prospect");
            var product1 = new Product("Potato", 90);
            var product2 = new Product("Cucumber", 130);
            shop1.AddProduct(product1);

            var product3 = new Product("Banana");
            Assert.Catch<ShopException>(() =>
            {
                shop1.AddProduct(product3);
            });

            Assert.AreEqual(1, shop1.GetProductAmount(product1));
            Assert.AreEqual(0, shop1.GetProductAmount(product2));

            var person = new Customer("Andrew", 100_000_000);
            shop2.AddProduct(product2);

            Assert.AreEqual(1, shop2.GetProductAmount(product2));

            shop2.Buy(person, product2, 1);
            shop2.AddProduct(product1);

            Assert.AreEqual(0, shop2.GetProductAmount(product2));
            Assert.Catch<ShopException>(() =>
            {
                shop1.Buy(person, product2, 12);
            });
        }

        [Test]
        public void SetAndChangeProductPricesInShop_PriceHasChanged()
        {
            var shop1 = new Shop("Diksi", "Nevskiy Prospect");
            var product1 = new Product("Potato", 90);
            var product2 = new Product("Cucumber", 130);

            shop1.AddProduct(product1);
            shop1.SetProductPrice(product1, 111);

            Assert.AreEqual(111, shop1.GetProductPrice(product1));

            var person = new Customer("Andrew", 100_000);

            shop1.Buy(person, product1, 1);

            Assert.Catch<ShopException>(() =>
            {
                shop1.SetProductPrice(product2, 100);
            });

            shop1.AddProduct(product1);

            Assert.Catch<ShopException>(() =>
            {
                shop1.SetProductPrice(product1, -1);
            });
        }

        [Test]
        public void BuyInProductWithCheapestPrice_GetShopWithCheapestPriceAndBuy()
        {
            var shopManager = new ShopManager();

            Shop shop1 = shopManager.CreateShop("Pyaterochka", "Kronverskiy prospect");
            Shop shop2 = shopManager.CreateShop("Diksi", "Lomonosova street");
            Shop shop3 = shopManager.CreateShop("Perekrestok", "Birzhevaya street");

            Product product = shopManager.RegisterProduct("Potato", 123);

            shop1.AddProduct(product);
            shop2.AddProduct(product);
            shop3.AddProduct(product);

            shop1.SetProductPrice(product, 100);
            shop2.SetProductPrice(product, 200);
            shop3.SetProductPrice(product, 300);

            Assert.AreEqual(shop1, shopManager.GetShopWithLowestProductPrice(product, 1));

            Product newProduct = shopManager.RegisterProduct("Tomato", 100);

            Assert.AreEqual(null, shopManager.GetShopWithLowestProductPrice(newProduct, 1));
        }

        [Test]
        public void BuyShipments_MoneyTransactionsProductsAmountChanged()
        {
            var shopManager = new ShopManager();

            Shop shop1 = shopManager.CreateShop("Pyaterochka", "Kronverskiy prospect");
            Product product1 = shopManager.RegisterProduct("Potato", 60);
            Product product2 = shopManager.RegisterProduct("Tomato", 90);
            Product product3 = shopManager.RegisterProduct("Cucumber", 75);
            Product product4 = shopManager.RegisterProduct("Peach", 120);
            Product product5 = shopManager.RegisterProduct("Chery", 240);

            shop1.AddProduct(product1, 2);
            shop1.AddProduct(product2, 3);
            shop1.AddProduct(product3, 4);
            shop1.AddProduct(product4, 1);
            shop1.AddProduct(product5, 2);

            var person = new Customer("Andrew", 100_000_000);
            var purchaseOrder = new List<ProductAmount>
            {
                new ProductAmount(product1, 2),
                new ProductAmount(product2, 2),
                new ProductAmount(product3, 3)
            };

            shop1.BuyShipments(person, purchaseOrder);
            Assert.AreEqual(0, shop1.GetProductAmount(product1));
            Assert.AreEqual(1, shop1.GetProductAmount(product2));
            Assert.AreEqual(1, shop1.GetProductAmount(product3));
            Assert.Catch<ShopException>(() =>
            {
                shop1.BuyShipments(person, purchaseOrder);
            });
        }
    }
}