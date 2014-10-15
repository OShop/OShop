using OShop.Models;
using System.Globalization;

namespace OShop.ViewModels {
    public class ShoppingCartWidgetViewModel {
        public ShoppingCart Cart { get; set; }
        public NumberFormatInfo NumberFormat { get; set; }

        // Optional features
        public bool VatEnabled { get; set; }
    }
}