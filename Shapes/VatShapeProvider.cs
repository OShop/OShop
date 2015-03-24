using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;

namespace OShop.Shapes {
    [OrchardFeature("OShop.VAT")]
    public class VatShapeProvider : IShapeTableProvider {
        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Price")
                .OnDisplaying(displaying => {
                    displaying.ShapeMetadata.Alternates.Add("Price__Vat");
                    displaying.ShapeMetadata.Alternates.Add("Price_" + displaying.ShapeMetadata.DisplayType + "__Vat");
                });
        }
    }
}