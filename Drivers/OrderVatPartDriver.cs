using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.VAT")]
    public class OrderVatPartDriver : ContentPartDriver<OrderVatPart> {
        private const string TemplateName = "Parts/Order.Vat";

        protected override string Prefix { get { return "OrderVat"; } }

        public OrderVatPartDriver() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Display(OrderVatPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Order_Vat_SubTotal", () => shapeHelper.Parts_Order_SubTotal(
                    Label: T("VAT"),
                    SubTotal: part.SubTotal))
            );
        }

        protected override DriverResult Editor(OrderVatPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Order_Vat_Edit", () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix)
                );
        }

        protected override DriverResult Editor(OrderVatPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper) {
            return Editor(part, shapeHelper);
        }
    }
}