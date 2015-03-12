using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace OShop.Models {
    public class CustomerOrderPart : ContentPart<CustomerOrderPartRecord> {
        internal readonly LazyField<CustomerPart> _customer = new LazyField<CustomerPart>();

        public CustomerPart Customer {
            get { return _customer.Value; }
            set { _customer.Value = value; }
        }
    }
}