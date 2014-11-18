using Orchard;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Mvc.Html;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [Admin]
    [OrchardFeature("OShop.Locations")]
    public class LocationsAdminController : Controller
    {
        private readonly ILocationsService _locationService;
        private readonly IShippingService _shippingService;
        private readonly ISiteService _siteService;
        private readonly IFeatureManager _featureManager;

        public LocationsAdminController(
            ILocationsService locationService,
            IOrchardServices services,
            ISiteService siteService,
            IFeatureManager featureManager,
            IShapeFactory shapeFactory,
            IShippingService shippingService = null
            ) {
            _locationService = locationService;
            _siteService = siteService;
            _featureManager = featureManager;
            Shape = shapeFactory;

            _shippingService = shippingService;

            Services = services;

            T = NullLocalizer.Instance;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        #region Countries
        public ActionResult Index(LocationsCountriesIndexViewModel model, PagerParameters pagerParameters) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var countries = _locationService.GetCountries();

            if (model.ZoneId > 0) {
                countries = countries.Where(c => c.ShippingZoneRecord != null && c.ShippingZoneRecord.Id == model.ZoneId);
            }

            switch (model.Filter) {
                case LocationsFilter.Disabled:
                    countries = countries.Where(c => !c.Enabled);
                    break;
                case LocationsFilter.Enabled:
                    countries = countries.Where(c => c.Enabled);
                    break;
            }

            var pagerShape = Shape.Pager(pager).TotalItemCount(countries.Count());

            var viewModel = new LocationsCountriesIndexViewModel() {
                DefaultCountryId = _locationService.GetDefaultCountryId(),
                Countries = countries.Skip(pager.GetStartIndex()).Take(pager.PageSize),
                Pager = pagerShape,
                BulkAction = LocationsBulkAction.None,
                Filter = model.Filter,
                ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>(),
                ZoneId = model.ZoneId,
                // Optional features
                ShippingEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.Shipping").Any()
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.BulkEdit")]
        public ActionResult IndexBulkEdit(LocationsCountriesIndexViewModel model, IEnumerable<int> itemIds, string returnUrl) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            if (itemIds != null && model.BulkAction != LocationsBulkAction.None) {
                int counter = 0;
                foreach (int itemId in itemIds) {
                    var country = _locationService.GetCountry(itemId);
                    if (country != null) {
                        switch (model.BulkAction) {
                            case LocationsBulkAction.Enable:
                                country.Enabled = true;
                                break;
                            case LocationsBulkAction.Disable:
                                country.Enabled = false;
                                break;
                            case LocationsBulkAction.Remove:
                                _locationService.DeleteCountry(country);
                                break;
                        }
                        counter++;
                    }
                }

                switch (model.BulkAction) {
                    case LocationsBulkAction.Enable:
                        Services.Notifier.Information(T.Plural("One country successfully enabled.", "{0} countries successfully enabled.", counter));
                        break;
                    case LocationsBulkAction.Disable:
                        Services.Notifier.Information(T.Plural("One country successfully disabled.", "{0} countries successfully disabled.", counter));
                        break;
                    case LocationsBulkAction.Remove:
                        Services.Notifier.Information(T.Plural("One country successfully deleted.", "{0} countries successfully deleted.", counter));
                        break;
                }
            }

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.BulkSetZone")]
        public ActionResult IndexBulkSetZone(LocationsCountriesIndexViewModel model, IEnumerable<int> itemIds, string returnUrl) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            if (itemIds != null) {
                ShippingZoneRecord zone = null;

                if (model.BulkZoneId > 0) {
                    zone = _shippingService.GetZone(model.BulkZoneId);
                    if (zone == null) {
                        Services.Notifier.Warning(T("Unknown zone : Unable to update contry zones"));
                        return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
                    }
                }

                int counter = 0;
                foreach (int itemId in itemIds) {
                    var country = _locationService.GetCountry(itemId);
                    if (country != null) {
                        country.ShippingZoneRecord = zone;
                        counter++;
                    }
                }

                Services.Notifier.Information(T.Plural("One country zone successfully updated.", "{0} countries zones successfully updated.", counter));

            }

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        public ActionResult AddCountry() {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            var model = new LocationsCountriesAddViewModel();

            model.ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddCountry(LocationsCountriesAddViewModel model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid) {
                var record = new LocationsCountryRecord() {
                    Name = model.Name,
                    IsoCode = model.IsoCode,
                    AddressFormat = model.AddressFormat,
                    Enabled = model.Enabled,
                };

                if (_shippingService != null) {
                    record.ShippingZoneRecord = _shippingService.GetZone(model.ShippingZoneId);
                }

                _locationService.AddCountry(record);
                Services.Notifier.Information(T("Country {0} successfully added.", model.Name));
                return RedirectToAction("Index");
            }

            model.ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>();

            return View(model);
        }

        public ActionResult EditCountry(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetCountry(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            var model = new LocationsCountriesEditViewModel() {
                Name = record.Name,
                IsoCode = record.IsoCode,
                AddressFormat = record.AddressFormat,
                Enabled = record.Enabled,
                ShippingZoneId = record.ShippingZoneRecord != null ? record.ShippingZoneRecord.Id : 0
            };

            model.ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>();

            return View(model);
        }

        [HttpPost]
        public ActionResult EditCountry(int id, LocationsCountriesEditViewModel model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetCountry(model.Id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            if (ModelState.IsValid) {
                record.Name = model.Name;
                record.IsoCode = model.IsoCode;
                record.AddressFormat = model.AddressFormat;
                record.Enabled = model.Enabled;

                if (_shippingService != null) {
                    record.ShippingZoneRecord = _shippingService.GetZone(model.ShippingZoneId);
                }

                Services.Notifier.Information(T("Country {0} successfully updated.", model.Name));
            }

            model.ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>();

            return View(model);
        }

        public ActionResult EnableCountry(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetCountry(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            record.Enabled = true;

            Services.Notifier.Information(T("Country {0} successfully enabled.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        public ActionResult DisableCountry(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetCountry(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            record.Enabled = false;

            Services.Notifier.Information(T("Country {0} successfully disabled.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        public ActionResult DeleteCountry(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetCountry(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            _locationService.DeleteCountry(record);

            Services.Notifier.Information(T("Country {0} successfully deleted.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        public ActionResult SetDefaultCountry(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage Countries")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetCountry(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            _locationService.SetDefaulCountry(record);

            Services.Notifier.Information(T("Country {0} successfully defined as default.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        #endregion

        #region States
        public ActionResult States(LocationsStatesIndexViewModel model, PagerParameters pagerParameters) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var states = _locationService.GetStates();

            if (model.CountryId > 0) {
                states = states.Where(s => s.LocationsCountryRecord.Id == model.CountryId);
            }

            if (model.ZoneId > 0) {
                states = states.Where(c => c.ShippingZoneRecord != null && c.ShippingZoneRecord.Id == model.ZoneId);
            }

            switch (model.Filter) {
                case LocationsFilter.Disabled:
                    states = states.Where(c => !c.Enabled);
                    break;
                case LocationsFilter.Enabled:
                    states = states.Where(c => c.Enabled);
                    break;
            }

            var pagerShape = Shape.Pager(pager).TotalItemCount(states.Count());

            var viewModel = new LocationsStatesIndexViewModel() {
                Countries = _locationService.GetCountries(),
                States = states.Skip(pager.GetStartIndex()).Take(pager.PageSize),
                Pager = pagerShape,
                BulkAction = LocationsBulkAction.None,
                Filter = model.Filter,
                ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>(),
                CountryId = model.CountryId,
                ZoneId = model.ZoneId,
                // Optional features
                ShippingEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.Shipping").Any()
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("States")]
        [FormValueRequired("submit.BulkEdit")]
        public ActionResult StatesBulkEdit(LocationsStatesIndexViewModel model, IEnumerable<int> itemIds, string returnUrl) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            if (itemIds != null && model.BulkAction != LocationsBulkAction.None) {
                int counter = 0;
                foreach (int itemId in itemIds) {
                    var state = _locationService.GetState(itemId);
                    if (state != null) {
                        switch (model.BulkAction) {
                            case LocationsBulkAction.Enable:
                                state.Enabled = true;
                                break;
                            case LocationsBulkAction.Disable:
                                state.Enabled = false;
                                break;
                            case LocationsBulkAction.Remove:
                                _locationService.DeleteState(state);
                                break;
                        }
                        counter++;
                    }
                }

                switch (model.BulkAction) {
                    case LocationsBulkAction.Enable:
                        Services.Notifier.Information(T.Plural("One state successfully enabled.", "{0} states successfully enabled.", counter));
                        break;
                    case LocationsBulkAction.Disable:
                        Services.Notifier.Information(T.Plural("One state successfully disabled.", "{0} states successfully disabled.", counter));
                        break;
                    case LocationsBulkAction.Remove:
                        Services.Notifier.Information(T.Plural("One state successfully deleted.", "{0} states successfully deleted.", counter));
                        break;
                }
            }

            return this.RedirectLocal(returnUrl, () => RedirectToAction("states"));
        }

        [HttpPost, ActionName("States")]
        [FormValueRequired("submit.BulkSetZone")]
        public ActionResult StatesBulkSetZone(LocationsStatesIndexViewModel model, IEnumerable<int> itemIds, string returnUrl) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            if (itemIds != null) {
                ShippingZoneRecord zone = null;

                if (model.BulkZoneId > 0) {
                    zone = _shippingService.GetZone(model.BulkZoneId);
                    if (zone == null) {
                        Services.Notifier.Warning(T("Unknown zone : Unable to update state zones"));
                        return this.RedirectLocal(returnUrl, () => RedirectToAction("States"));
                    }
                }

                int counter = 0;
                foreach (int itemId in itemIds) {
                    var state = _locationService.GetState(itemId);
                    if (state != null) {
                        state.ShippingZoneRecord = zone;
                        counter++;
                    }
                }

                Services.Notifier.Information(T.Plural("One state zone successfully updated.", "{0} states zones successfully updated.", counter));

            }

            return this.RedirectLocal(returnUrl, () => RedirectToAction("States"));
        }

        public ActionResult AddState() {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            var model = new LocationsStatesAddViewModel();

            model.ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>();
            model.Countries = _locationService.GetCountries();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddState(LocationsStatesAddViewModel model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid) {
                var record = new LocationsStateRecord() {
                    Name = model.Name,
                    IsoCode = model.IsoCode,
                    Enabled = model.Enabled,
                    LocationsCountryRecord = _locationService.GetCountry(model.CountryId)
                };

                if (_shippingService != null) {
                    record.ShippingZoneRecord = _shippingService.GetZone(model.ShippingZoneId);
                }

                _locationService.AddState(record);
                Services.Notifier.Information(T("State {0} successfully added.", model.Name));
                return RedirectToAction("States");
            }

            model.Countries = _locationService.GetCountries();
            model.ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>();

            return View(model);
        }

        public ActionResult EditState(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetState(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            var model = new LocationsStatesEditViewModel() {
                Name = record.Name,
                IsoCode = record.IsoCode,
                Enabled = record.Enabled,
                CountryId = record.LocationsCountryRecord != null ? record.LocationsCountryRecord.Id : 0,
                ShippingZoneId = record.ShippingZoneRecord != null ? record.ShippingZoneRecord.Id : 0
            };

            model.Countries = _locationService.GetCountries();
            model.ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>();

            return View(model);
        }

        [HttpPost]
        public ActionResult EditState(int id, LocationsStatesEditViewModel model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetState(model.Id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            if (ModelState.IsValid) {
                record.Name = model.Name;
                record.IsoCode = model.IsoCode;
                record.Enabled = model.Enabled;
                record.LocationsCountryRecord = model.CountryId > 0 ? _locationService.GetCountry(model.CountryId) : null;

                if (_shippingService != null) {
                    record.ShippingZoneRecord = _shippingService.GetZone(model.ShippingZoneId);
                }

                Services.Notifier.Information(T("State {0} successfully updated.", model.Name));
            }

            model.Countries = _locationService.GetCountries();
            model.ShippingZones = _shippingService != null ? _shippingService.GetZones() : new List<ShippingZoneRecord>();

            return View(model);
        }

        public ActionResult EnableState(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetState(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            record.Enabled = true;

            Services.Notifier.Information(T("State {0} successfully enabled.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("States"));
        }

        public ActionResult DisableState(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetState(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            record.Enabled = false;

            Services.Notifier.Information(T("State {0} successfully disabled.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("States"));
        }

        public ActionResult DeleteState(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage States")))
                return new HttpUnauthorizedResult();

            var record = _locationService.GetState(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            _locationService.DeleteState(record);

            Services.Notifier.Information(T("State {0} successfully deleted.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("States"));
        }

        #endregion

    }
}