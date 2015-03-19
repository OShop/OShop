using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class CustomerOrderPartRecord : ContentPartRecord {
        public virtual Int32 CustomerId { get; set; }
        public virtual Int32 CustomerVersionId { get; set; }
        public virtual Int32 BillingAddressId { get; set; }
        public virtual Int32 BillingAddressVersionId { get; set; }
        public virtual Int32 ShippingAddressId { get; set; }
        public virtual Int32 ShippingAddressVersionId { get; set; }
    }
}