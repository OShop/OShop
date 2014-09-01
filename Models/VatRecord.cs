
using System.ComponentModel.DataAnnotations;
namespace OShop.Models {
    public class VatRecord {
        public virtual int Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual decimal Rate { get; set; }
    }
}