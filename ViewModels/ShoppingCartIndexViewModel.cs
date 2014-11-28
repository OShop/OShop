using OShop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class ShoppingCartIndexViewModel {
        public ShoppingCart Cart { get; set; }
        public NumberFormatInfo NumberFormat { get; set; }

        // Optional features
        public bool VatEnabled { get; set; }
        public bool CheckoutEnabled { get; set; }
        public bool ExpressCheckoutEnabled { get; set; }

        // Location selection
        public IEnumerable<LocationsCountryRecord> Countries { get; set; }
        public IEnumerable<LocationsStateRecord> States { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }

        // Selected addresses
        public LocationsCountryRecord BillingCountry { get; set; }
        public LocationsStateRecord BillingState { get; set; }
        public LocationsCountryRecord ShippingCountry { get; set; }
        public LocationsStateRecord ShippingState { get; set; }

        // Shipping selection
        public IEnumerable<ShippingProviderOption> ShippingProviders { get; set; }
        public int ShippingProviderId { get; set; }

        // Cart content update
        public ShoppingCartItemUpdateViewModel[] CartItems { get; set; }

        // Submit action
        public String Action { get; set; }
    }

    public class ShoppingCartItemUpdateViewModel {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public bool IsRemoved { get; set; }
    }
}