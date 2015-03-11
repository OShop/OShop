using OShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class CustomerAddressEditViewModel {
        public CustomerAddressEditViewModel() { }

        public CustomerAddressEditViewModel(CustomerAddressPart part) {
                AddressAlias = part.AddressAlias;
                IsDefault = part.IsDefault;
                Company = part.Company;
                FirstName = part.FirstName;
                LastName = part.LastName;
                Address1 = part.Address1;
                Address2 = part.Address2;
                Zipcode = part.Zipcode;
                City = part.City;
                CountryId = part.CountryId;
                StateId = part.StateId;
                CustomerId = part.Customer != null ? part.Customer.ContentItem.Id : 0;
        }
        
        public Boolean IsDefault { get; set; }
        [Required]
        public String AddressAlias { get; set; }
        public String Company { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        [Required]
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        [Required]
        public String Zipcode { get; set; }
        [Required]
        public String City { get; set; }
        public IEnumerable<LocationsCountryRecord> Countries { get; set; }
        public IEnumerable<LocationsStateRecord> States { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CustomerId { get; set; }
    }
}