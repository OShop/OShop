using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Models {
    public class OrderShippingPart : ContentPart<OrderShippingPartRecord>, IOrderSubTotal {
        internal readonly LazyField<IEnumerable<OrderDetail>> _shippingDetails = new LazyField<IEnumerable<OrderDetail>>();
        internal readonly LazyField<OrderAddressRecord> _shippingAddress = new LazyField<OrderAddressRecord>();

        public ShippingStatus ShippingStatus {
            get { return (ShippingStatus)this.Retrieve(x => x.ShippingStatus); }
            set { this.Store(x => x.ShippingStatus, (int)value); }
        }

        public int ShippingAddressId {
            get { return this.Retrieve(x => x.ShippingAddressId); }
            set { this.Store(x => x.ShippingAddressId, value); }
        }

        public OrderAddressRecord ShippingAddress {
            get { return _shippingAddress.Value; }
            set { _shippingAddress.Value = value; }
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