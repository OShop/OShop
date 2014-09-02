using System.ComponentModel.DataAnnotations;

namespace OShop.ViewModels {
    public class VatCreateViewModel {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Rate { get; set; }
    }
}