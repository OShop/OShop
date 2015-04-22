using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Orders")]
    public class OrderPartDriver : ContentPartDriver<OrderPart> {
        private readonly ILocationsService _locationsService;

        private const string TemplateName = "Parts/Order";

        protected override string Prefix { get { return "Order"; } }

        public OrderPartDriver(
            ILocationsService locationsService) {
            _locationsService = locationsService;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Display(OrderPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Order", () => shapeHelper.Parts_Order(
                    ContentPart: part)),
                ContentShape("Parts_Order_Summary", () => shapeHelper.Parts_Order_Summary(
                    ContentPart: part)),
                ContentShape("Parts_Order_SummaryAdmin", () => shapeHelper.Parts_Order_SummaryAdmin(
                    ContentPart: part)),
                ContentShape("Parts_Order_Reference", () => shapeHelper.Parts_Order_Reference(
                    ContentPart: part)),
                ContentShape("Parts_Order_BillingAddress", () => shapeHelper.Parts_Order_BillingAddress(
                    ContentPart: part,
                    Address: _locationsService.FormatAddress(part.BillingAddress))),
                ContentShape("Parts_Order_Status", () => shapeHelper.Parts_Order_Status(
                    ContentPart: part)),
                ContentShape("Parts_Order_Total", () => shapeHelper.Parts_Order_SubTotal(
                    Label: T("Order total"),
                    SubTotal: part.OrderTotal))
            );
        }

        protected override DriverResult Editor(OrderPart part, dynamic shapeHelper) {
            var model = new OrderPartEditViewModel() {
                Reference = part.Reference,
                OrderStatus = part.OrderStatus,
            };

            if (part.BillingAddress != null) {
                model.BillingAddress = _locationsService.FormatAddress(part.BillingAddress);
            }

            return Combined(
                ContentShape("Parts_Order_Edit", () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: model,
                    Prefix: Prefix))
            );
        }

        protected override DriverResult Editor(OrderPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }
    }
}