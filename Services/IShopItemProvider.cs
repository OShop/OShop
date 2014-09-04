using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;

namespace OShop.Services {
    public interface IShopItemProvider : IDependency {
        Int16 Priority { get; }
        void GetItems(IEnumerable<ShoppingCartItemRecord> CartRecord, out List<ShoppingCartItem> CartItems);
    }
}
