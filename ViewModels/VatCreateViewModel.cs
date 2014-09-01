using System.ComponentModel.DataAnnotations;

namespace OShop.ViewModels {
    public class VatCreateViewModel {
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual decimal Rate { get; set; }
    }
}