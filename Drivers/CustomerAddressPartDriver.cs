using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerAddressPartDriver : ContentPartDriver<CustomerAddressPart> {
        private const string TemplateName = "Parts/CustomerAddress";

        protected override string Prefix { get { return "CustomerAddress"; } }

        protected override DriverResult Display(CustomerAddressPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_CustomerAddress", () => shapeHelper.Parts_Customer(
                    ContentPart: part));
        }

        // GET
        protected override DriverResult Editor(CustomerAddressPart part, dynamic shapeHelper) {
            return ContentShape("Parts_CustomerAddress_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(CustomerAddressPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }
    }
}