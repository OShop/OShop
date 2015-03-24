using Orchard.ContentManagement.Records;
using System;

namespace OShop.Models {
    public class VatRatePartRecord : ContentPartVersionRecord {
        public virtual String Name { get; set; }
        public virtual Decimal Rate { get; set; }
    }
}