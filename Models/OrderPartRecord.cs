using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    [OrchardFeature("OShop.Orders")]
    public class OrderPartRecord : ContentPartRecord {
        public virtual string Reference { get; set; }
        public virtual int OrderStatus { get; set; }
        public virtual string Logs { get; set; }
    }

}