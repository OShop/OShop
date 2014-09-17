using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class ShoppingCartUpdateViewModel {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public bool IsRemoved { get; set; }
    }
}