using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class OrderPartEditViewModel {
        public string Reference { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string BillingAddress { get; set; }
    }
}