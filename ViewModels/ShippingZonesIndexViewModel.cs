using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class ShippingZonesIndexViewModel {
        public IEnumerable<ShippingZoneRecord> Zones { get; set; }

        public dynamic Pager { get; set; }
        public ShippingZoneBulkAction BulkAction { get; set; }
        public ShippingZoneFilter Filter { get; set; }
    }

    public enum ShippingZoneBulkAction {
        None,
        Enable,
        Disable,
        Remove
    }

    public enum ShippingZoneFilter {
        All,
        Enabled,
        Disabled
    }
}