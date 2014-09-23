using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingProviderPartRecord : ContentPartRecord {
        public virtual VatRecord VatRecord { get; set; }
    }
}