using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;

namespace OShop.Shapes {
    [OrchardFeature("OShop.VAT")]
    public class OShopShapeProvider : IShapeTableProvider {
        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Price")
                .OnDisplaying(displaying => {
                    displaying.ShapeMetadata.Alternates.Add("Parts_" + displaying.ShapeMetadata.DisplayType);
                });
        }
    }
}