using Orchard.ContentManagement.Records;

namespace OShop.Models {
    public class OrderShippingPartRecord : ContentPartRecord {
        public virtual int ShippingStatus { get; set; }
        public virtual int ShippingAddressId { get; set; }
    }
}