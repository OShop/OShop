using System.ComponentModel.DataAnnotations;

namespace OShop.Models {
    public class ShippingZoneRecord {
        public virtual int Id { get; set; }
        public virtual bool Enabled { get; set; }
        [Required]
        public virtual string Name { get; set; }
    }
}