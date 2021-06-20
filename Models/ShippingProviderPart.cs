using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace OShop.Models {
    public class ShippingProviderPart : ContentPart {
        internal LazyField<IEnumerable<ShippingOptionRecord>> _options = new LazyField<IEnumerable<ShippingOptionRecord>>();

        public IEnumerable<ShippingOptionRecord> Options {
            get { return _options.Value; }
        }

    }
}