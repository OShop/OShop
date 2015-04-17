using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderAddressRecord : IOrderAddress {
        public virtual int Id { get; set; }
        public virtual String Company { get; set; }
        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual String Address1 { get; set; }
        public virtual String Address2 { get; set; }
        public virtual String Zipcode { get; set; }
        public virtual String City { get; set; }
        public virtual LocationsCountryRecord Country { get; set; }
        public virtual LocationsStateRecord State { get; set; }
    }
}