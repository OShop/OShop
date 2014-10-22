using Orchard.ContentManagement.Records;
using System;
using System.ComponentModel.DataAnnotations;

namespace OShop.Models {
    public class CustomerAddressPartRecord : ContentPartRecord {
        [Required]
        public virtual String AddressAlias { get; set; }
        public virtual String Company { get; set; }
        [Required]
        public virtual String FirstName { get; set; }
        [Required]
        public virtual String LastName { get; set; }
        [Required]
        public virtual String Address1 { get; set; }
        public virtual String Address2 { get; set; }
        [Required]
        public virtual String Zipcode { get; set; }
        [Required]
        public virtual String City { get; set; }
        public virtual Int32 LocationsCountryId { get; set; }
        public virtual Int32 LocationsStateId { get; set; }
    }
}