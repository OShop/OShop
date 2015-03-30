using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace OShop.Models {
    public class OrderShippingPart : ContentPart<OrderShippingPartRecord> {
        internal readonly LazyField<ShippingProviderPart> _provider = new LazyField<ShippingProviderPart>();

        public ShippingStatus ShippingStatus {
            get { return (ShippingStatus)this.Retrieve(x => x.ShippingStatus); }
            set { this.Store(x => x.ShippingStatus, (int)value); }
        }

        internal int ProviderId {
            get { return Retrieve(x => x.ProviderId); }
            set { Store(x => x.ProviderId, value); }
        }

        internal int ProviderVersionId {
            get { return Retrieve(x => x.ProviderVersionId); }
            set { Store(x => x.ProviderVersionId, value); }
        }

        public ShippingProviderPart Provider {
            get { return _provider.Value; }
            set { _provider.Value = value; }
        }

        public decimal Price {
            get { return this.Retrieve(x => x.Price); }
            set { this.Store(x => x.Price, value); }
        }
    }

    public enum ShippingStatus : int {
        NotRequired = 0,
        Pending = 1,
        Shipped = 2,
        Delivered = 3
    }
}