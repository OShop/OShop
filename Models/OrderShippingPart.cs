using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Models {
    public class OrderShippingPart : ContentPart<OrderShippingPartRecord>, IOrderSubTotal {
        internal readonly LazyField<IEnumerable<OrderDetail>> _shippingDetails = new LazyField<IEnumerable<OrderDetail>>();

        public ShippingStatus ShippingStatus {
            get { return (ShippingStatus)this.Retrieve(x => x.ShippingStatus); }
            set { this.Store(x => x.ShippingStatus, (int)value); }
        }

        public IEnumerable<OrderDetail> ShippingDetails {
            get { return _shippingDetails.Value; }
        }

        public decimal SubTotal {
            get { return ShippingDetails.Sum(d => d.Total); }
        }
    }

    public enum ShippingStatus : int {
        NotRequired = 0,
        Pending = 1,
        Shipped = 2,
        Delivered = 3
    }
}