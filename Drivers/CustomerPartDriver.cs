using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.UI.Admin;
using OShop.Models;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerPartDriver : ContentPartDriver<CustomerPart> {

        protected override string Prefix { get { return "Customer"; } }

        public CustomerPartDriver(
            IOrchardServices orchardServices
            ) {
            Services = orchardServices;
        }

        public IOrchardServices Services { get; private set; }

        protected override DriverResult Display(CustomerPart part, string displayType, dynamic shapeHelper) {
            if (AdminFilter.IsApplied(Services.WorkContext.HttpContext.Request.RequestContext)) {
                return Combined(
                    ContentShape("Parts_Customer_Admin", () => shapeHelper.Parts_Customer_Admin(
                        ContentPart: part)
                    ),
                    ContentShape("Parts_Customer_Addresses_Admin", () => shapeHelper.Parts_Customer_Addresses_Admin(
                        ContentPart: part)
                    )
                );
            }
            else {
                return Combined(
                    ContentShape("Parts_Customer", () => shapeHelper.Parts_Customer(
                        ContentPart: part)
                    ),
                    ContentShape("Parts_Customer_Addresses", () => shapeHelper.Parts_Customer_Addresses(
                        ContentPart: part)
                    )
                );
            }
        }

        // GET
        protected override DriverResult Editor(CustomerPart part, dynamic shapeHelper) {
            if (AdminFilter.IsApplied(Services.WorkContext.HttpContext.Request.RequestContext)) {
                return ContentShape("Parts_Customer_AdminEdit",
                    () => shapeHelper.EditorTemplate(
                        TemplateName: "Parts/Customer.AdminEdit",
                        Model: part,
                        Prefix: Prefix)
                );
            }
            else {
                return ContentShape("Parts_Customer_Edit",
                    () => shapeHelper.EditorTemplate(
                        TemplateName: "Parts/Customer",
                        Model: part,
                        Prefix: Prefix)
                );
            }
        }

        // POST
        protected override DriverResult Editor(CustomerPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }
    }
}