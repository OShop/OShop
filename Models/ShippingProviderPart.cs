using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using Orchard.Data.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShippingProviderPart : ContentPart<ShippingProviderPartRecord> {
        internal LazyField<IEnumerable<ShippingOptionRecord>> _options = new LazyField<IEnumerable<ShippingOptionRecord>>();

        public IEnumerable<ShippingOptionRecord> Options {
            get { return _options.Value; }
        }

        public VatRecord VAT {
            get { return Record.VatRecord; }
            set { Record.VatRecord = value; }
        }

    }
}