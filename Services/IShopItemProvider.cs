using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;

namespace OShop.Services {
    public interface IShopItemProvider : IDependency {
        Int16 Priority { get; }
        void GetItems(IEnumerable<ShoppingCartItem> CartItems, out IList<IShopItem> ShopItems);
    }
}
