using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace OShop.Models {
    [OrchardFeature("OShop.Orders")]
    public class OrderPartRecord : ContentPartRecord {
        public virtual string Reference { get; set; }
        public virtual int BillingAddressId { get; set; }
        public virtual int OrderStatus { get; set; }
        public virtual decimal OrderTotal { get; set; }
        public virtual string Logs { get; set; }
    }

}