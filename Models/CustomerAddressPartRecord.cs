using Orchard.ContentManagement.Records;
using System;

namespace OShop.Models {
    public class CustomerAddressPartRecord : ContentPartRecord {
        public virtual CustomerPartRecord CustomerPartRecord { get; set; }
        public virtual String Label { get; set; }
        public virtual LocationsCountryRecord LocationsCountryRecord { get; set; }
        public virtual LocationsStateRecord LocationsStateRecord { get; set; }
    }
}