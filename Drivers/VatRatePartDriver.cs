using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.VAT")]
    public class VatRatePartDriver : ContentPartDriver<VatRatePart> {
        private const string TemplateName = "Parts/VatRate";

        protected override string Prefix { get { return "VatRate"; } }

        protected override DriverResult Display(VatRatePart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_VatRate", () => shapeHelper.Parts_VatRate(
                    ContentPart: part
                )
            );
        }

        // GET
        protected override DriverResult Editor(VatRatePart part, dynamic shapeHelper) {
            return ContentShape("Parts_VatRate_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix
                )
            );
        }

        // POST
        protected override DriverResult Editor(VatRatePart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }
    }
}