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
            builder.Describe("ShoppingCart_CartItems")
                .OnDisplaying(displaying => {
                    displaying.ShapeMetadata.Alternates.Add("ShoppingCart_CartItems__Vat");
                });
            builder.Describe("ShoppingCart_Widget")
                .OnDisplaying(displaying => {
                    displaying.ShapeMetadata.Alternates.Add("ShoppingCart_Widget__Vat");
                });
        }
    }
}