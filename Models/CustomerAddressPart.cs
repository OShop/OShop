using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class CustomerAddressPart : ContentPart<CustomerAddressPartRecord> {
        [Required]
        public String AddressAlias {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
    }
}