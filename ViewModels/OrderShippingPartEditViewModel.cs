using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class OrderShippingPartEditViewModel {
        public ShippingStatus ShippingStatus { get; set; }
        public string ShippingAddress { get; set; }
    }
}