using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderAddress : IOrderAddress {
        public string Company { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Zipcode { get; set; }

        public string City { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }
    }
}