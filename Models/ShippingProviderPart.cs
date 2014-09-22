using Orchard.ContentManagement;
using Orchard.Data.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShippingProviderPart : ContentPart<ShippingProviderPartRecord> {
        [CascadeAllDeleteOrphan]
        public virtual IList<ShippingOptionRecord> Options { get; set; }

        internal ShippingProviderPart() {
            Options = new List<ShippingOptionRecord>();
        }
    }
}