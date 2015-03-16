using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerOrderPartDriver : ContentPartDriver<CustomerOrderPart> {

        private const string TemplateName = "Parts/CustomerOrder";

        public CustomerOrderPartDriver() {

        }

        protected override string Prefix { get { return "CustomerOrder"; } }

        protected override DriverResult Display(CustomerOrderPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_CustomerOrder", () => shapeHelper.Parts_CustomerOrder(
                ContentPart: part
            ));
        }

        // GET
        protected override DriverResult Editor(CustomerOrderPart part, dynamic shapeHelper) {
            return ContentShape("Parts_CustomerOrder_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix));
        }
    }
}