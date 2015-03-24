using OShop.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OShop.ViewModels {
    public class VatEditViewModel {
        public IEnumerable<VatRatePart> VatRates { get; set; }
        [Required]
        public int SelectedRateId { get; set; }
    }
}