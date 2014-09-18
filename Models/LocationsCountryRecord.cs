using Orchard.Data.Conventions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OShop.Models {
    public class LocationsCountryRecord {
        public virtual int Id { get; set; }
        public virtual bool Enabled { get; set; }
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string IsoCode { get; set; }
        public virtual ShippingZoneRecord ShippingZone { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<LocationsStateRecord> States { get; set; }

        internal LocationsCountryRecord() {
            States = new List<LocationsStateRecord>();
        }
    }
}