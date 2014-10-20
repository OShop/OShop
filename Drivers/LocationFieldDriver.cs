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

namespace OShop.Drivers {
    [OrchardFeature("OShop.Locations")]
    public class LocationFieldDriver : ContentFieldDriver<LocationField> {
        private readonly ILocationsService _locationService;

        private const string TemplateName = "Fields/Location";

        public LocationFieldDriver(ILocationsService locationService) {
            _locationService = locationService;
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
            return ContentShape("Fields_Location_Edit", GetDifferentiator(field, part),
                () => {
                    var model = new LocationFieldViewModel() {
                        CountryId = field.CountryId,
                        StateId = field.StateId,
                        Countries = _locationService.GetEnabledCountries(),
                        States = _locationService.GetEnabledStates(field.CountryId)
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