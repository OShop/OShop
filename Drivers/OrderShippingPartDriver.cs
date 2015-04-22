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
        private readonly ILocationsService _locationsService;

        protected override string Prefix { get { return "OrderShipping"; } }

        public OrderShippingPartDriver(
            ILocationsService locationsService) {
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
                    ContentPart: part)),
                ContentShape("Parts_Order_Shipping_SubTotal", () => shapeHelper.Parts_Order_SubTotal(
                    Label: T("Shipping"),
                    SubTotal: part.SubTotal))
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
                ContentShape("Parts_Order_Shipping_Edit", () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Order.Shipping",
                    Model: model,
                    Prefix: Prefix)),
                ContentShape("Parts_Order_ShippingDetails_Edit", () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Order.ShippingDetails",
                    Model: part,
                    Prefix: Prefix))
            );
        }

        protected override DriverResult Editor(OrderShippingPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }

    }
}