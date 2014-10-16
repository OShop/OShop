using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class CustomerAddressPart : ContentPart<CustomerAddressPartRecord> {
        public CustomerPartRecord CustomerPartRecord {
            get { return Record.CustomerPartRecord; }
            set { Record.CustomerPartRecord = value; }
        }
        public String Label {
            get { return this.Retrieve(x => x.Label); }
            set { this.Store(x => x.Label, value); }
        }
    }
}