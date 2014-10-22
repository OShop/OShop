using Orchard.ContentManagement;
using System;

namespace OShop.Models {
    public class CustomerAddressPart : ContentPart<CustomerAddressPartRecord> {
        public String AddressAlias {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public String Company {
            get { return this.Retrieve(x => x.Company); }
            set { this.Store(x => x.Company, value); }
        }
        public String FirstName {
            get { return this.Retrieve(x => x.FirstName); }
            set { this.Store(x => x.FirstName, value); }
        }
        public String LastName {
            get { return this.Retrieve(x => x.LastName); }
            set { this.Store(x => x.LastName, value); }
        }
        public String Address1 {
            get { return this.Retrieve(x => x.Address1); }
            set { this.Store(x => x.Address1, value); }
        }
        public String Address2 {
            get { return this.Retrieve(x => x.Address2); }
            set { this.Store(x => x.Address2, value); }
        }
        public String Zipcode {
            get { return this.Retrieve(x => x.Zipcode); }
            set { this.Store(x => x.Zipcode, value); }
        }
        public String City {
            get { return this.Retrieve(x => x.City); }
            set { this.Store(x => x.City, value); }
        }
        public Int32 CountryId {
            get { return this.Retrieve(x => x.LocationsCountryId); }
            set { this.Store(x => x.LocationsCountryId, value); }
        }
        public Int32 StateId {
            get { return this.Retrieve(x => x.LocationsStateId); }
            set { this.Store(x => x.LocationsStateId, value); }
        }
    }
}