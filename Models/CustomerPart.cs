using Orchard.ContentManagement;
using System;

namespace OShop.Models {
    public class CustomerPart : ContentPart<CustomerPartRecord> {
        public String FirstName {
            get { return this.Retrieve(x => x.FirstName); }
            set { this.Store(x => x.FirstName, value); }
        }
        public String LastName {
            get { return this.Retrieve(x => x.LastName); }
            set { this.Store(x => x.LastName, value); }
        }
        public DateTime CreatedUtc {
            get { return this.Retrieve(x => x.CreatedUtc); }
            set { this.Store(x => x.CreatedUtc, value); }
        }
    }
}