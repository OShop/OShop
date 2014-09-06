using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartProductPartDriver : ContentPartDriver<ProductPart> {
        protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Product_AddToCart", () => shapeHelper.Parts_Product_AddToCart(
                    ContentPart: part));
        }
    }
}