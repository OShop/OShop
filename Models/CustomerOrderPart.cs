using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace OShop.Models {
    public class CustomerOrderPart : ContentPart<CustomerOrderPartRecord>, IBillingAddress, IShippingAddress {
        internal readonly LazyField<CustomerPart> _customer = new LazyField<CustomerPart>();
        internal readonly LazyField<CustomerAddressPart> _billingAddress = new LazyField<CustomerAddressPart>();
        internal readonly LazyField<CustomerAddressPart> _shippingAddress = new LazyField<CustomerAddressPart>();

        internal int CustomerId {
            get { return Retrieve(x => x.CustomerId); }
            set { Store(x => x.CustomerId, value); }
        }

        internal int CustomerVersionId {
            get { return Retrieve(x => x.CustomerVersionId); }
            set { Store(x => x.CustomerVersionId, value); }
        }

        internal int BillingAddressId {
            get { return Retrieve(x => x.BillingAddressId); }
            set { Store(x => x.BillingAddressId, value); }
        }

        internal int BillingAddressVersionId {
            get { return Retrieve(x => x.BillingAddressVersionId); }
            set { Store(x => x.BillingAddressVersionId, value); }
        }

        internal int ShippingAddressId {
            get { return Retrieve(x => x.ShippingAddressId); }
            set { Store(x => x.ShippingAddressId, value); }
        }

        internal int ShippingAddressVersionId {
            get { return Retrieve(x => x.ShippingAddressVersionId); }
            set { Store(x => x.ShippingAddressVersionId, value); }
        }

        public CustomerPart Customer {
            get { return _customer.Value; }
            set { _customer.Value = value; }
        }

        public CustomerAddressPart BillingAddress {
            get { return _billingAddress.Value; }
            set { _billingAddress.Value = value; }
        }

        public CustomerAddressPart ShippingAddress {
            get { return _shippingAddress.Value; }
            set { _shippingAddress.Value = value; }
        }

        IOrderAddress IBillingAddress.BillingAddress {
            get { return BillingAddress; }
        }

        IOrderAddress IShippingAddress.ShippingAddress {
            get { return ShippingAddress; }
        }
    }
}