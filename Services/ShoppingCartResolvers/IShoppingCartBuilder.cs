using System;
using Orchard;
using OShop.Models;

namespace OShop.Services.ShoppingCartResolvers {
    public interface IShoppingCartBuilder : IDependency {
        Int32 Priority { get; }
        void BuildCart(IShoppingCartService ShoppingCartService, ShoppingCart Cart);
    }
}
