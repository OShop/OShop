using Orchard.ContentManagement.Records;
using System;

namespace OShop.Models {
    public class CustomerAddressPartRecord : ContentPartRecord {
        public virtual String AddressAlias { get; set; }
    }
}