using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace OShop.Models {
    public class CustomerOrderPart : ContentPart<CustomerOrderPartRecord> {
        internal readonly LazyField<CustomerPart> _customer = new LazyField<CustomerPart>();

        internal int CustomerId {
            get { return Retrieve(x => x.CustomerId); }
            set { Store(x => x.CustomerId, value); }
        }

        public CustomerPart Customer {
            get { return _customer.Value; }
            set { _customer.Value = value; }
        }
    }
}