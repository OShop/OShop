using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using OShop.Models;
using OShop.Services;
using System.Linq;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Orders")]
    public class OrderPartDriver : ContentPartDriver<OrderPart> {
        private readonly ICurrencyProvider _currencyProvider;
        private readonly ILocationsService _locationsService;

        private const string TemplateName = "Parts/Order";

        protected override string Prefix { get { return "Order"; } }

        public OrderPartDriver(
            ICurrencyProvider currencyProvider,
            ILocationsService locationsService
            ) {
            _currencyProvider = currencyProvider;
            _locationsService = locationsService;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Display(OrderPart part, string displayType, dynamic shapeHelper) {
            IBillingAddress billingAddress = part.As<IBillingAddress>();

            return Combined(
                ContentShape("Parts_Order", () => shapeHelper.Parts_Order(
                    ContentPart: part)),
                ContentShape("Parts_Order_Summary", () => shapeHelper.Parts_Order_Summary(
                    ContentPart: part,
                    NumberFormat: _currencyProvider.NumberFormat)),
                ContentShape("Parts_Order_Reference", () => shapeHelper.Parts_Order_Reference(
                    ContentPart: part)),
                billingAddress != null && billingAddress.BillingAddress != null ?
                    ContentShape("Parts_Order_BillingAddress", () => shapeHelper.Parts_Order_BillingAddress(
                        ContentPart: part,
                        Address: _locationsService.FormatAddress(billingAddress.BillingAddress)))
                        : null,
                ContentShape("Parts_Order_Status", () => shapeHelper.Parts_Order_Status(
                    ContentPart: part)),
                ContentShape("Parts_Order_Total", () => shapeHelper.Parts_Order_SubTotal(
                    Label: T("Order total"),
                    SubTotal: part.OrderTotal,
                    NumberFormat: _currencyProvider.NumberFormat))
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