using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Shipping")]
    public class OrderShippingPartDriver : ContentPartDriver<OrderShippingPart> {
        private readonly ILocationsService _locationsService;

        private const string TemplateName = "Parts/OrderShipping";

        public OrderShippingPartDriver(ILocationsService locationsService) {
            _locationsService = locationsService;
        }

        protected override string Prefix { get { return "OrderShipping"; } }

        protected override DriverResult Display(OrderShippingPart part, string displayType, dynamic shapeHelper) {
            if (part.ShippingStatus == ShippingStatus.NotRequired) {
                return null;
            }

            IShippingAddress shippingAddress = part.As<IShippingAddress>();

            return Combined(
                shippingAddress != null && shippingAddress.ShippingAddress != null ?
                    ContentShape("Parts_Order_ShippingAddress", () => shapeHelper.Parts_Order_ShippingAddress(
                        ContentPart: part,
                        Address: _locationsService.FormatAddress(shippingAddress.ShippingAddress)))
                        : null,
                ContentShape("Parts_Order_ShippingStatus", () => shapeHelper.Parts_Order_ShippingStatus(
                    ContentPart: part)),
                ContentShape("Parts_Order_ShippingProvider", () => shapeHelper.Parts_Order_ShippingProvider(
                    ContentPart: part))
            );
        }

    }
}