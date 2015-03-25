using OShop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class CheckoutIndexViewModel {
        public Int32 BillingAddressId { get; set; }
        public Int32 ShippingAddressId { get; set; }
        public Int32 ShippingProviderId { get; set; }
    }
}