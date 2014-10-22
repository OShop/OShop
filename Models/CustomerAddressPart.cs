using Orchard.ContentManagement;
using System;

namespace OShop.Models {
    public class CustomerAddressPart : ContentPart<CustomerAddressPartRecord> {
        public String AddressAlias {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public String Company {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public String FirstName {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public String LastName {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public String Address1 {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public String Address2 {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public String Zipcode {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public String City {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public LocationsCountryRecord Country {
            get { return Record.LocationsCountryRecord; }
            set { Record.LocationsCountryRecord = value; }
        }
        public LocationsStateRecord State {
            get { return Record.LocationsStateRecord; }
            set { Record.LocationsStateRecord = value; }
        }
    }
}