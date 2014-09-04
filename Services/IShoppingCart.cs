using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Services {
    public interface IShoppingCart {
        int Id { get; }
        int Guid { get; }
        DateTime ModifiedUtc { get; }
        int? OwnerId { get; }
        IEnumerable<ShoppingCartItem> Items { get; }
        ShoppingCartItem Add(int ItemId, string ItemType = "Product", int Quantity = 1);
        ShoppingCartItem UpdateQuantity(int Id, int Quantity);
        ShoppingCartItem Remove(int Id);
        ShoppingCartItem Empty();
    }
}
