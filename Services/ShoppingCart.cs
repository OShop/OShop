using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCart : IShoppingCart {

        public ShoppingCart() {

        }

        public int Id {
            get { throw new NotImplementedException(); }
        }

        public int Guid {
            get { throw new NotImplementedException(); }
        }

        public DateTime ModifiedUtc {
            get { throw new NotImplementedException(); }
        }

        public int? OwnerId {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<Models.ShoppingCartItem> Items {
            get { throw new NotImplementedException(); }
        }

        public Models.ShoppingCartItem Add(int ItemId, string ItemType = "Product", int Quantity = 1) {
            throw new NotImplementedException();
        }

        public Models.ShoppingCartItem UpdateQuantity(int Id, int Quantity) {
            throw new NotImplementedException();
        }

        public Models.ShoppingCartItem Remove(int Id) {
            throw new NotImplementedException();
        }

        public Models.ShoppingCartItem Empty() {
            throw new NotImplementedException();
        }
    }
}