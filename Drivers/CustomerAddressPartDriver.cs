using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string TemplateName = "Parts/CustomerAddress";

        protected override string Prefix { get { return "CustomerAddress"; } }

        public CustomerAddressPartDriver(
            ILocationsService locationService,
            IHttpContextAccessor httpContextAccessor
            ) {
            _locationService = locationService;
            _httpContextAccessor = httpContextAccessor;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Display(CustomerAddressPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_CustomerAddress", () => shapeHelper.Parts_Customer(
                    ContentPart: part));
        }

        // GET
        protected override DriverResult Editor(CustomerAddressPart part, dynamic shapeHelper) {
            var httpContext = _httpContextAccessor.Current();
            int countryId = part.Country != null ? part.Country.Id : 0;

            if (httpContext.Request.Form[Prefix + ".CountryId"] != null) {
                countryId = Int32.Parse(httpContext.Request.Form[Prefix + ".CountryId"]);
            }
            else if (countryId <= 0) {
                countryId = _locationService.GetDefaultCountryId();
            }

            var model = new CustomerAddressEditViewModel() {
                AddressAlias = part.AddressAlias,
                Company = part.Company,
                FirstName = part.FirstName,
                LastName = part.LastName,
                Address1 = part.Address1,
                Address2 = part.Address2,
                Zipcode = part.Zipcode,
                City = part.City,
                CountryId = countryId,
                StateId = part.State != null ? part.State.Id : 0,
                Countries = _locationService.GetEnabledCountries(),
                States = _locationService.GetEnabledStates(countryId)
            };

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
            var model = new CustomerAddressEditViewModel();
            if (updater.TryUpdateModel(model, Prefix, null, null)) {
                if (model.CountryId <= 0) {
                    updater.AddModelError("CountryId", T("Please select your country."));
                }
                else if (_locationService.GetStates(model.CountryId).Any() && model.StateId <= 0) {
                    updater.AddModelError("StateId", T("Please select your state."));
                }
                else {
                    part.AddressAlias = model.AddressAlias;
                    part.Company = model.Company;
                    part.FirstName = model.FirstName;
                    part.LastName = model.LastName;
                    part.Address1 = model.Address1;
                    part.Address2 = model.Address2;
                    part.Zipcode = model.Zipcode;
                    part.City = model.City;
                    part.Country = _locationService.GetCountry(model.CountryId);
                    part.State = model.StateId > 0 ? _locationService.GetState(model.StateId) : null;
                }
            }

            return Editor(part, shapeHelper);
        }
    }
}