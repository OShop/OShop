using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace OShop.Models {
    public class VatPart : ContentPart<VatPartRecord> {
        internal readonly LazyField<VatRatePart> _vatRate = new LazyField<VatRatePart>();

        internal Int32 VatRateId {
            get { return this.Retrieve(x => x.VatRateId); }
            set { this.Store(x => x.VatRateId, value); }
        }

        public VatRatePart VatRate {
            get { return _vatRate.Value; }
            set { _vatRate.Value = value; }
        }
    }
}