using Orchard.Environment.Extensions;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.Locations")]
    public class LocationsController : Controller
    {
        private readonly ILocationsService _locationService;

        public LocationsController(ILocationsService locationService) {
            _locationService = locationService;
        }

        /// <summary>
        /// Return states list for selected country
        /// </summary>
        /// <param name="id">CountryId</param>
        /// <returns>Country states</returns>
        public ActionResult States(int id)
        {
            var states = _locationService.GetEnabledStates(id).Select(s => new { id = s.Id, name = s.Name}).ToArray();

            return Json(states, JsonRequestBehavior.AllowGet);
        }
    }
}