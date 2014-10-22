using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using OShop.Fields;
using OShop.Services;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.ViewModels;
using OShop.Settings;
using Orchard.Localization;
using Orchard.Mvc;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Locations")]
    public class LocationFieldDriver : ContentFieldDriver<LocationField> {
        private readonly ILocationsService _locationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string TemplateName = "Fields/Location";

        public LocationFieldDriver(
            ILocationsService locationService,
            IHttpContextAccessor httpContextAccessor
            ) {
            _locationService = locationService;
            _httpContextAccessor = httpContextAccessor;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private static string GetPrefix(ContentField field, ContentPart part) {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(LocationField field, ContentPart part) {
            return field.Name;
        }

        protected override DriverResult Display(ContentPart part, LocationField field, string displayType, dynamic shapeHelper) {
            return ContentShape("Fields_Location", GetDifferentiator(field, part), () => {
                var country = _locationService.GetCountry(field.CountryId);
                var state = _locationService.GetState(field.StateId);
                return shapeHelper.Fields_Location(
                    ContentField: field,
                    Country: country,
                    State: state
                );
            });
        }

        protected override DriverResult Editor(ContentPart part, LocationField field, dynamic shapeHelper) {
            var httpContext = _httpContextAccessor.Current();
            Int32? countryId = null;
            String countryFieldName = GetPrefix(field, part) + ".CountryId";
            if (httpContext.Request.Form[countryFieldName] != null) {
                countryId = Int32.Parse(httpContext.Request.Form[countryFieldName]);
            }
            else if (field.CountryId <= 0) {
                countryId = _locationService.GetDefaultCountryId();
            }

            return ContentShape("Fields_Location_Edit", GetDifferentiator(field, part),
                () => {
                    var model = new LocationFieldViewModel() {
                        Settings = field.PartFieldDefinition.Settings.GetModel<LocationFieldSettings>(),
                        CountryId = countryId.HasValue ? countryId.Value : field.CountryId,
                        StateId = field.StateId,
                        Countries = _locationService.GetEnabledCountries(),
                        States = _locationService.GetEnabledStates(countryId.HasValue ? countryId.Value : field.CountryId)
                    };
                    return shapeHelper.EditorTemplate(
                        TemplateName: TemplateName,
                        Model: model,
                        Prefix: GetPrefix(field, part)
                    );
                }
            );
        }

        protected override DriverResult Editor(ContentPart part, LocationField field, IUpdateModel updater, dynamic shapeHelper) {
            if (updater.TryUpdateModel(field, GetPrefix(field, part), null, null)) {
                var settings = field.PartFieldDefinition.Settings.GetModel<LocationFieldSettings>();

                if (settings.Required) {
                    if(field.CountryId <= 0) {
                        updater.AddModelError(GetPrefix(field, part), T("Please select your country."));
                    }
                    else if (_locationService.GetStates(field.CountryId).Any() && field.StateId <= 0) {
                        updater.AddModelError(GetPrefix(field, part), T("Please select your state."));
                    }
                }
            }

            return Editor(part, field, shapeHelper);
        }
    }
}