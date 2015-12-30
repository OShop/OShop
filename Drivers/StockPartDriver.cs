using System.Runtime.CompilerServices;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Stocks")]
    public class StockPartDriver : ContentPartDriver<StockPart> {
        private const string TemplateName = "Parts/Stock";

        protected override string Prefix { get { return "Stock"; } }

        public StockPartDriver() { }

        protected override DriverResult Display(StockPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Stock", () => shapeHelper.Parts_Stock(ContentPart: part));
        }

        // GET
        protected override DriverResult Editor(StockPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Stock_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix)
            );
        }

        // POST
        protected override DriverResult Editor(StockPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}