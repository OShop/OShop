using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class OrdersIndexViewModel {
        public IEnumerable<dynamic> Orders { get; set; }
        public dynamic Pager { get; set; }
    }
}