using Orchard;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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