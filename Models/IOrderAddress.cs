using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Models {
    public interface IOrderAddress {
        String Company { get; set; }
        String FirstName { get; set; }
        String LastName { get; set; }
        String Address1 { get; set; }
        String Address2 { get; set; }
        String Zipcode { get; set; }
        String City { get; set; }
        LocationsCountryRecord Country { get; set; }
        LocationsStateRecord State { get; set; }
    }
}
