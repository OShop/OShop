using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Environment.Extensions;
using Orchard.Security;
using Orchard.UI.Admin;
using OShop.Models;
using OShop.Permissions;
using OShop.Services;
using OShop.ViewModels;
using System.Collections.Generic;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerPartDriver : ContentPartDriver<CustomerPart> {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomersService _customersService;

        protected override string Prefix { get { return "Customer"; } }

        public CustomerPartDriver(
            IAuthenticationService authenticationService,
            ICustomersService customersService,
            IOrchardServices orchardServices
            ) {
            _authenticationService = authenticationService;
            Services = orchardServices;
            _customersService = customersService;
        }

        public IOrchardServices Services { get; private set; }

        protected override DriverResult Display(CustomerPart part, string displayType, dynamic shapeHelper) {
            bool allowEdit = Services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.EditContent, part);
            return Combined(
                ContentShape("Parts_Customer", () => shapeHelper.Parts_Customer(
                    ContentPart: part)
                ),
                ContentShape("Parts_Customer_Addresses", () => shapeHelper.Parts_Customer_Addresses(
                    ContentPart: part)
                )
            );
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
            if (updater.TryUpdateModel(part, Prefix, null, null)) {
                if (part.User == null) {
                    var user = _authenticationService.GetAuthenticatedUser();
                    if (user != null) {
                        part.UserId = user.ContentItem.Id;
                    }
                }
            }
            return Editor(part, shapeHelper);
        }
    }
}