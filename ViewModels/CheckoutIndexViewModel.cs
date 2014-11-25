using OShop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class CheckoutIndexViewModel {
        public Boolean ShippingRequired { get; set; }
        public IEnumerable<CustomerAddressPart> Addresses { get; set; }
        public Int32 BillingAddressId { get; set; }
        public dynamic BillingAddress { get; set; }
        public Int32 ShippingAddressId { get; set; }
        public dynamic ShippingAddress { get; set; }

        public NumberFormatInfo NumberFormat { get; set; }
        public bool VatEnabled { get; set; }

        public IEnumerable<ShippingProviderOption> ShippingProviders { get; set; }
        public int ShippingProviderId { get; set; }
    }
}