using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.UI.Notify;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerAddressPartDriver : ContentPartDriver<CustomerAddressPart> {
        private readonly ILocationsService _locationService;
        private readonly ICustomersService _customersService;

        private const string TemplateName = "Parts/CustomerAddress";

        protected override string Prefix { get { return "CustomerAddress"; } }

        public CustomerAddressPartDriver(
            ILocationsService locationService,
            ICustomersService customersService,
            IOrchardServices orchardServices
            ) {
            _locationService = locationService;
            _customersService = customersService;
            Services = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public IOrchardServices Services { get; private set; }

        protected override DriverResult Display(CustomerAddressPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_CustomerAddress", () => shapeHelper.Parts_CustomerAddress(
                    ContentPart: part,
                    FormatedAddress: _locationService.FormatAddress(part)
                )
            );
        }

        // GET
        protected override DriverResult Editor(CustomerAddressPart part, dynamic shapeHelper) {
            var httpContext = Services.WorkContext.HttpContext;
            var model = new CustomerAddressEditViewModel(part);
            model.Countries = _locationService.GetEnabledCountries();
            if (part.CountryId <= 0) {
                part.CountryId = _locationService.GetDefaultCountryId();
            }

            model.States = _locationService.GetEnabledStates(part.CountryId);

            int customerId;
            if (Int32.TryParse(httpContext.Request.Params["CustomerId"], out customerId) && Services.Authorizer.Authorize(Permissions.CustomersPermissions.ManageCustomerAccounts)) {
                model.CustomerId = customerId;
            }

            return ContentShape("Parts_CustomerAddress_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: model,
                    Prefix: Prefix
                )
            );
        }

        // POST
        protected override DriverResult Editor(CustomerAddressPart part, IUpdateModel updater, dynamic shapeHelper) {
            var httpContext = Services.WorkContext.HttpContext;

            if (updater.TryUpdateModel(part, Prefix, null, null)) {
                if (part.CountryId <= 0) {
                    updater.AddModelError("CountryId", T("Please select your country."));
                }
                else {
                    var states = _locationService.GetStates(part.CountryId);
                    if (states.Any()) {
                        if (part.StateId <= 0 || !states.Where(s => s.Id == part.StateId).Any()) {
                            updater.AddModelError("StateId", T("Please select your state."));
                        }
                    }
                }

                int customerId;
                if (Int32.TryParse(httpContext.Request.Form[Prefix + ".CustomerId"], out customerId) && Services.Authorizer.Authorize(Permissions.CustomersPermissions.ManageCustomerAccounts)) {
                    part.Customer = _customersService.GetCustomer(customerId);
                }
                else if (part.Customer == null) {
                    part.Customer = _customersService.GetCustomer();
                }
            }

            return Editor(part, shapeHelper);
        }
    }
}