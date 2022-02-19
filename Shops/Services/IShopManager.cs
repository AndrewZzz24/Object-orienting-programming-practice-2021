// @author Andrew Zmushko (andrewzmushko@gmail.com)

using Shops.Entities;

namespace Shops.Services
{
    public interface IShopManager
    {
        Shop CreateShop(string name, string address);

        Shop GetShop(int id);
        Shop GetShop(string name);
        Product RegisterProduct(string productName, int price);
    }
}