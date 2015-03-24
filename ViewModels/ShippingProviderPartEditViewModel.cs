using OShop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class ShippingProviderPartEditViewModel {
        public ShippingProviderPart Part { get; set; }
        public NumberFormatInfo NumberFormat { get; set; }
    }
}