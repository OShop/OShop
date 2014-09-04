using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.Products")]
    public class ProductService : IShopItemProvider {
        public ProductService() {

        }

        public short Priority {
            get { return 0; }
        }

        public void GetItems(IEnumerable<ShoppingCartItemRecord> CartRecord, out List<ShoppingCartItem> CartItems) {
            throw new NotImplementedException();
        }
    }
}