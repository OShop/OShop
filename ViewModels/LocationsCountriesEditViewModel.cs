using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.ViewModels {
    public class LocationsCountriesEditViewModel {
        public IEnumerable<ShippingZoneRecord> ShippingZones { get; set; }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public int ShippingZoneId { get; set; }
    }
}