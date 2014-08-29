using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    [OrchardFeature("OShop.Products")]
    public class ProductPartRecord : ContentPartVersionRecord {
        public virtual decimal UnitPrice { get; set; }
        public virtual string SKU { get; set; }
        public virtual VatRecord VatRecord { get; set; }
    }
}