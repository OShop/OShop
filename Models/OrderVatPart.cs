using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Models {
    public class OrderVatPart : ContentPart, IOrderSubTotal {
        internal readonly LazyField<IList<TaxAmount>> _vatAmounts = new LazyField<IList<TaxAmount>>();

        public IList<TaxAmount> VatAmounts {
            get { return _vatAmounts.Value; }
        }

        public decimal SubTotal {
            get { return VatAmounts.Sum(va => va.Tax.Rate * va.TaxBase); }
        }
    }
}