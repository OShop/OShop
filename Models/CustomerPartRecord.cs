using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class CustomerPartRecord : ContentPartRecord {
        public CustomerPartRecord() {
            CustomerAddressPartRecords = new List<CustomerAddressPartRecord>();
        }

        public virtual Int32 UserId { get; set; }
        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual Int32 DefaultAddressId { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<CustomerAddressPartRecord> CustomerAddressPartRecords { get; set; }
    }
}