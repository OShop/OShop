using OShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.ViewModels {
    public class LocationsCountriesAddViewModel {
        public IEnumerable<ShippingZoneRecord> ShippingZones { get; set; }

        public bool Enabled { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string IsoCode { get; set; }
        [Required]
        public string AddressFormat { get; set; }
        public int ShippingZoneId { get; set; }
    }
}