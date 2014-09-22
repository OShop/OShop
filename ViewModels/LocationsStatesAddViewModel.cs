using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class LocationsStatesAddViewModel {
        public IEnumerable<ShippingZoneRecord> ShippingZones { get; set; }
        public IEnumerable<LocationsCountryRecord> Countries { get; set; }

        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public int CountryId { get; set; }
        public int ShippingZoneId { get; set; }
    }
}