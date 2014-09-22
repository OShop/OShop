using Orchard.Data.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShippingOptionRecord {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ShippingZoneRecord ShippingZoneRecord { get; set; }
        public virtual ShippingProviderPartRecord ShippingProviderPartRecord { get; set; }
        public virtual int Priority { get; set; }
        [StringLengthMax]
        public virtual string Data { get; set; }
        public virtual decimal Price { get; set; }
        public virtual VatRecord VatRecord { get; set; }
    }
}