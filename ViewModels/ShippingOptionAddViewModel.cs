using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class ShippingOptionAddViewModel {
        public int ShippingProviderId { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public IEnumerable<ShippingZoneRecord> ShippingZones { get; set; }
        public int ShippingZoneId { get; set; }
        public int Priority { get; set; }
        public decimal Price { get; set; }
    }
}