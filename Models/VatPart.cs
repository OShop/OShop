using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class VatPart : ContentPart<VatPartRecord> {
        internal readonly LazyField<VatRatePart> _vatRate = new LazyField<VatRatePart>();

        internal Int32 VatRateId {
            get { return this.Retrieve(x => x.VatRateId); }
            set { this.Store(x => x.VatRateId, value); }
        }

        internal Int32 VatRateVersionId {
            get { return this.Retrieve(x => x.VatRateVersionId); }
            set { this.Store(x => x.VatRateVersionId, value); }
        }

        public VatRatePart VatRate {
            get { return _vatRate.Value; }
            set { _vatRate.Value = value; }
        }
    }
}