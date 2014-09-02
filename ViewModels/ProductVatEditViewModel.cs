using OShop.Models;
using System.Collections.Generic;

namespace OShop.ViewModels {
    public class ProductVatEditViewModel {
        public IEnumerable<VatRecord> VatRates { get; set; }
        public string SelectedVatId { get; set; }
    }
}