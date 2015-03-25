using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Models {
    public interface IOrderAddress {
        String Company { get; }
        String FirstName { get; }
        String LastName { get; }
        String Address1 { get; }
        String Address2 { get; }
        String Zipcode { get; }
        String City { get; }
        LocationsCountryRecord Country { get; }
        LocationsStateRecord State { get; }
    }
}
