using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;

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

        protected override DriverResult Editor(OrderShippingPart part, dynamic shapeHelper) {
            var model = new OrderShippingPartEditViewModel() {
                ShippingStatus = part.ShippingStatus,
            };

            if (part.ShippingAddress != null) {
                model.ShippingAddress = _locationsService.FormatAddress(part.ShippingAddress);
            }

            return Combined(
                ContentShape("Parts_OrderShipping_Edit", () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: model,
                    Prefix: Prefix))
            );
        }

        protected override DriverResult Editor(OrderShippingPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }

    }
}