using OShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class LocationsStatesAddViewModel {
        public IEnumerable<ShippingZoneRecord> ShippingZones { get; set; }
        public IEnumerable<LocationsCountryRecord> Countries { get; set; }

        public bool Enabled { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string IsoCode { get; set; }
        public int CountryId { get; set; }
        public int ShippingZoneId { get; set; }
    }
}