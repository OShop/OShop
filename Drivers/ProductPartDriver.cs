using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Products")]
    public class ProductPartDriver : ContentPartDriver<ProductPart> {
        private const string TemplateName = "Parts/Product";

        public ProductPartDriver() {
        }

        protected override string Prefix { get { return "Product"; } }

        protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Product", () => shapeHelper.Parts_Product(
                    ContentPart: part)),
                ContentShape("Price", () => shapeHelper.Price(
                    ContentPart: part))
            );
        }

        // GET
        protected override DriverResult Editor(ProductPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Product_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix)
            );
        }

        // POST
        protected override DriverResult Editor(ProductPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}