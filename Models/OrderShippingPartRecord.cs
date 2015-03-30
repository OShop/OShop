using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderShippingPartRecord : ContentPartRecord {
        public virtual int ShippingStatus { get; set; }
        public virtual int ProviderId { get; set; }
        public virtual int ProviderVersionId { get; set; }
        public virtual decimal Price { get; set; }
    }
}