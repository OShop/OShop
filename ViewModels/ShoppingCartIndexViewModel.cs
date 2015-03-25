using OShop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class ShoppingCartIndexViewModel {
        // Location selection
        public int CountryId { get; set; }
        public int StateId { get; set; }

        // Shipping selection
        public int ShippingProviderId { get; set; }

        // Cart content update
        public ShoppingCartItemUpdateViewModel[] CartItems { get; set; }
    }

    public class ShoppingCartItemUpdateViewModel {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public bool IsRemoved { get; set; }
    }
}