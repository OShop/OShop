using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Drivers
{
    [OrchardFeature("OShop.Shipping")]
    public class ShippingPartDriver : ContentPartDriver<ShippingPart> {
        private const string TemplateName = "Parts/Shipping";

        protected override string Prefix { get { return "Shipping"; } }

        protected override DriverResult Display(ShippingPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Shipping", () =>
                shapeHelper.Parts_Shipping(
                    ContentPart: part
                )
            );
        }

        // GET
        protected override DriverResult Editor(ShippingPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Shipping_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(ShippingPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}