using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace OShop.Models {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingPartRecord : ContentPartVersionRecord {
        public virtual bool RequiresShipping { get; set; }
        public virtual double Weight { get; set; }
        public virtual double Width { get; set; }
        public virtual double Height { get; set; }
        public virtual double Lenght { get; set; } 
    }
}