using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Helpers;
using OShop.Models;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.VAT")]
    public class ShippingVatResolver : IShoppingCartBuilder {
        public int Priority {
            get { return 100; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ShoppingCart Cart) {
            var shippingPrice = Cart.Shipping as IPrice;
            var shippingVat = (Cart.Shipping as IContent).GetVatRate();
            if (shippingPrice != null && shippingVat != null && shippingPrice.Price != 0) {
                Cart.AddTax(shippingVat, shippingPrice.Price);
            }
        }
    }
}