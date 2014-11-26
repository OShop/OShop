using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Orders")]
    public class OrderPartDriver : ContentPartDriver<OrderPart> {
        private readonly ICurrencyProvider _currencyProvider;

        private const string TemplateName = "Parts/Order";

        public OrderPartDriver(ICurrencyProvider currencyProvider) {
            _currencyProvider = currencyProvider;
        }

        protected override string Prefix { get { return "Order"; } }

        protected override DriverResult Display(OrderPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Order_Reference", () => shapeHelper.Parts_Order_Reference(
                    ContentPart: part)),
                ContentShape("Parts_Order_Customer", () => shapeHelper.Parts_Order_Customer(
                    ContentPart: part)),
                ContentShape("Parts_Order_BillingAddress", () => shapeHelper.Parts_Order_BillingAddress(
                    ContentPart: part)),
                ContentShape("Parts_Order_Status", () => shapeHelper.Parts_Order_Status(
                    ContentPart: part)),
                ContentShape("Parts_Order_Items", () => shapeHelper.Parts_Order_Items(
                    ContentPart: part,
                    NumberFormat: _currencyProvider.NumberFormat)),
                ContentShape("Parts_Order_Logs", () => shapeHelper.Parts_Order_Logs(
                    ContentPart: part))
            );
        }

        protected override DriverResult Editor(OrderPart part, dynamic shapeHelper) {
            return base.Editor(part, shapeHelper);
        }

        protected override DriverResult Editor(OrderPart part, IUpdateModel updater, dynamic shapeHelper) {
            return base.Editor(part, updater, shapeHelper);
        }
    }
}