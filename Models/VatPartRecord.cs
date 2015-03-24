using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class VatPartRecord : ContentPartVersionRecord {
        public virtual Int32 VatRateId { get; set; }
        public virtual Int32 VatRateVersionId { get; set; }
    }
}