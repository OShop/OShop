using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using OShop.Models;
using OShop.Services;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Shipping")]
    public class OrderShippingPartDriver : ContentPartDriver<OrderShippingPart> {
        private readonly ICurrencyProvider _currencyProvider;
        private readonly ILocationsService _locationsService;

        private const string TemplateName = "Parts/OrderShipping";

        protected override string Prefix { get { return "OrderShipping"; } }

        public OrderShippingPartDriver(
            ICurrencyProvider currencyProvider,
            ILocationsService locationsService
            ) {
            _currencyProvider = currencyProvider;
            _locationsService = locationsService;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Display(OrderShippingPart part, string displayType, dynamic shapeHelper) {
            if (part.ShippingStatus == ShippingStatus.NotRequired) {
                return null;
            }

            return Combined(
                ContentShape("Parts_Order_ShippingAddress", () => shapeHelper.Parts_Order_ShippingAddress(
                    ContentPart: part,
                    Address: _locationsService.FormatAddress(part.ShippingAddress))),
                ContentShape("Parts_Order_ShippingStatus", () => shapeHelper.Parts_Order_ShippingStatus(
                    ContentPart: part)),
                ContentShape("Parts_Order_ShippingProvider", () => shapeHelper.Parts_Order_ShippingProvider(
                    ContentPart: part,
                    NumberFormat: _currencyProvider.NumberFormat)),
                ContentShape("Parts_Order_Shipping_SubTotal", () => shapeHelper.Parts_Order_SubTotal(
                    Label: T("Shipping"),
                    SubTotal: part.SubTotal,
                    NumberFormat: _currencyProvider.NumberFormat))
            );
        }

    }
}