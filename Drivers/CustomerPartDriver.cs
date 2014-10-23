using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;
using System.Collections.Generic;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerPartDriver : ContentPartDriver<CustomerPart> {
        private readonly ICustomersService _customersService;

        private const string TemplateName = "Parts/Customer";

        protected override string Prefix { get { return "Customer"; } }

        public CustomerPartDriver(ICustomersService customersService) {
            _customersService = customersService;
        }

        protected override DriverResult Display(CustomerPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Customer", () => shapeHelper.Parts_Customer(
                    ContentPart: part)
                ),
                ContentShape("Parts_Customer_Addresses", () => shapeHelper.Parts_Customer_Addresses(
                    ContentPart: part,
                    Addresses: part.Owner != null ? _customersService.GetAddresses(part.Owner.Id) : new List<CustomerAddressPart>())
                )
            );
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