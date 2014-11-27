using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Shipping")]
    public class OrderShippingPartDriver : ContentPartDriver<OrderShippingPart> {
        private const string TemplateName = "Parts/OrderShipping";

        protected override string Prefix { get { return "OrderShipping"; } }

        protected override DriverResult Display(OrderShippingPart part, string displayType, dynamic shapeHelper) {
            if (part.ShippingStatus == ShippingStatus.NotRequired) {
                return null;
            }

            return Combined(
                ContentShape("Parts_Order_ShippingAddress", () => shapeHelper.Parts_Order_ShippingAddress(
                    ContentPart: part)),
                ContentShape("Parts_Order_ShippingStatus", () => shapeHelper.Parts_Order_ShippingStatus(
                    ContentPart: part)),
                ContentShape("Parts_Order_ShippingInfos", () => shapeHelper.Parts_Order_ShippingInfos(
                    ContentPart: part))
            );
        }

    }
}