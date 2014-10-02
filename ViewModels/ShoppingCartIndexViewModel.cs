using OShop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class ShoppingCartIndexViewModel {
        public IEnumerable<ShoppingCartItem> CartItems { get; set; }
        public NumberFormatInfo NumberFormat { get; set; }

        // Optional features
        public bool VatEnabled { get; set; }

        public IEnumerable<LocationsCountryRecord> Countries { get; set; }
        public IEnumerable<LocationsStateRecord> States { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }

        public IEnumerable<ShippingProviderOption> ShippingProviders { get; set; }
        public int ShippingProviderId { get; set; }
        public bool ShippingRequired { get; set; }
    }
}