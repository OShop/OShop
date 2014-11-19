using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class CheckoutIndexViewModel {
        public Boolean ShippingRequired { get; set; }
        public IEnumerable<CustomerAddressPart> Addresses { get; set; }
        public Int32 BillingAddressId { get; set; }
        public Int32 ShippingAddressId { get; set; }
    }
}