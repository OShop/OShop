using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Drivers {
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartWidgetPartDriver : ContentPartDriver<ShoppingCartWidgetPart> {

        public ShoppingCartWidgetPartDriver() {
        }

        protected override DriverResult Display(ShoppingCartWidgetPart part, string displayType, dynamic shapeHelper) {

            return ContentShape("Parts_ShoppingCartWidget", () => shapeHelper.Parts_ShoppingCartWidget());
        }
    }
}