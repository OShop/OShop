using Orchard.ContentManagement.Records;
using System;

namespace OShop.Models {
    public class CustomerAddressPartRecord : ContentPartVersionRecord {
        public virtual String AddressAlias { get; set; }
        public virtual String Company { get; set; }
        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual String Address1 { get; set; }
        public virtual String Address2 { get; set; }
        public virtual String Zipcode { get; set; }
        public virtual String City { get; set; }
        public virtual Int32 LocationsCountryId { get; set; }
        public virtual Int32 LocationsStateId { get; set; }

        public virtual Int32 CustomerId { get; set; }
    }
}