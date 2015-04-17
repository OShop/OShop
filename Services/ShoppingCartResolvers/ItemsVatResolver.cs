using Orchard.Environment.Extensions;
using OShop.Extensions;
using OShop.Models;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.VAT")]
    public class ItemsVatResolver : IShoppingCartBuilder {
        public int Priority {
            get { return 500; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ShoppingCart Cart) {
            foreach (var entry in Cart.Items) {
                var vat = entry.Item.GetVatRate();
                if (vat != null && entry.SubTotal() != 0) {
                    Cart.AddTax(vat, entry.SubTotal());
                }
            }
        }
    }
}