using OShop.Models;
using System.Collections.Generic;

namespace OShop.ViewModels {
    public class VatEditViewModel {
        public IEnumerable<VatRecord> VatRates { get; set; }
        public int SelectedVatId { get; set; }
    }
}