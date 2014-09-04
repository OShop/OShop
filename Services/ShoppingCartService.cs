using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartService : IShoppingCartService {

        public ShoppingCartService() {

        }

        public IShoppingCart GetCart() {
            throw new NotImplementedException();
        }

        public IShoppingCart GetCart(int Id) {
            throw new NotImplementedException();
        }

        public IShoppingCart GetCart(Guid Guid) {
            throw new NotImplementedException();
        }
    }
}