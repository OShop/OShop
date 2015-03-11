using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Utilities;
using Orchard.Core.Common.Models;
using Orchard.Security;
using System;
using System.ComponentModel.DataAnnotations;

namespace OShop.Models {
    public class CustomerAddressPart : ContentPart<CustomerAddressPartRecord>, IOrderAddress, ITitleAspect {
        internal readonly LazyField<CustomerPart> _customer = new LazyField<CustomerPart>();

        public string Title {
            get { return this.AddressAlias; }
        }

        [Required]
        public String AddressAlias {
            get { return this.Retrieve(x => x.AddressAlias); }
            set { this.Store(x => x.AddressAlias, value); }
        }
        public Boolean IsDefault {
            get { return this.Customer != null && this.Customer.DefaultAddressId == this.ContentItem.Id; }
            set {
                if (this.Customer != null) {
                    if (!value && this.Customer.DefaultAddressId == this.ContentItem.Id) {
                        this.Customer.DefaultAddressId = 0;
                    }
                    else if (value && this.Customer.DefaultAddressId != this.ContentItem.Id) {
                        this.Customer.DefaultAddressId = this.ContentItem.Id;
                    }
                }
            }
        }
        public String Company {
            get { return this.Retrieve(x => x.Company); }
            set { this.Store(x => x.Company, value); }
        }
        [Required]
        public String FirstName {
            get { return this.Retrieve(x => x.FirstName); }
            set { this.Store(x => x.FirstName, value); }
        }
        [Required]
        public String LastName {
            get { return this.Retrieve(x => x.LastName); }
            set { this.Store(x => x.LastName, value); }
        }
        [Required]
        public String Address1 {
            get { return this.Retrieve(x => x.Address1); }
            set { this.Store(x => x.Address1, value); }
        }
        public String Address2 {
            get { return this.Retrieve(x => x.Address2); }
            set { this.Store(x => x.Address2, value); }
        }
        [Required]
        public String Zipcode {
            get { return this.Retrieve(x => x.Zipcode); }
            set { this.Store(x => x.Zipcode, value); }
        }
        [Required]
        public String City {
            get { return this.Retrieve(x => x.City); }
            set { this.Store(x => x.City, value); }
        }
        [Required]
        public Int32 CountryId {
            get { return this.Retrieve(x => x.LocationsCountryId); }
            set { this.Store(x => x.LocationsCountryId, value); }
        }
        public Int32 StateId {
            get { return this.Retrieve(x => x.LocationsStateId); }
            set { this.Store(x => x.LocationsStateId, value); }
        }

        public CustomerPart Customer {
            get { return _customer.Value; }
            set { _customer.Value = value; }
        }
    }
}