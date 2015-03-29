using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Orders")]
    public class OrderPartDriver : ContentPartDriver<OrderPart> {
        private readonly ICurrencyProvider _currencyProvider;
        private readonly ILocationsService _locationsService;

        private const string TemplateName = "Parts/Order";

        public OrderPartDriver(
            ICurrencyProvider currencyProvider,
            ILocationsService locationsService
            ) {
            _currencyProvider = currencyProvider;
            _locationsService = locationsService;
        }

        protected override string Prefix { get { return "Order"; } }

        protected override DriverResult Display(OrderPart part, string displayType, dynamic shapeHelper) {
            IBillingAddress billingAddress = part.As<IBillingAddress>();

            return Combined(
                ContentShape("Parts_Order", () => shapeHelper.Parts_Order(
                    ContentPart: part)),
                ContentShape("Parts_Order_Summary", () => shapeHelper.Parts_Order_Summary(
                    ContentPart: part)),
                ContentShape("Parts_Order_Reference", () => shapeHelper.Parts_Order_Reference(
                    ContentPart: part)),
                billingAddress != null && billingAddress.BillingAddress != null ?
                    ContentShape("Parts_Order_BillingAddress", () => shapeHelper.Parts_Order_BillingAddress(
                        ContentPart: part,
                        Address: _locationsService.FormatAddress(billingAddress.BillingAddress)))
                        : null,
                ContentShape("Parts_Order_Status", () => shapeHelper.Parts_Order_Status(
                    ContentPart: part)),
                ContentShape("Parts_Order_Details", () => shapeHelper.Parts_Order_Details(
                    ContentPart: part,
                    NumberFormat: _currencyProvider.NumberFormat)),
                ContentShape("Parts_Order_Logs", () => shapeHelper.Parts_Order_Logs(
                    ContentPart: part))
            );
        }

        protected override DriverResult Editor(OrderPart part, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Order_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix))
            );
        }

        protected override DriverResult Editor(OrderPart part, IUpdateModel updater, dynamic shapeHelper) {

            return Editor(part, shapeHelper);
        }
    }
}