using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class ShoppingCartUpdateViewModel {
        public ShoppingCartItemUpdateViewModel[] CartItems { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int ShippingProviderId { get; set; }
        public String Action { get; set; }
    }

    public class ShoppingCartItemUpdateViewModel {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public bool IsRemoved { get; set; }
    }
}