using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerPartDriver : ContentPartDriver<CustomerPart> {
        private const string TemplateName = "Parts/Customer";

        protected override string Prefix { get { return "Customer"; } }

        protected override DriverResult Display(CustomerPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Customer", () => shapeHelper.Parts_Customer(
                    ContentPart: part));
        }

        // GET
        protected override DriverResult Editor(CustomerPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Customer_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(CustomerPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}