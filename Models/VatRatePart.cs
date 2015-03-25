using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using System;
using System.ComponentModel.DataAnnotations;

namespace OShop.Models {
    public class VatRatePart : ContentPart<VatRatePartRecord>, ITax, ITitleAspect {
        [Required]
        public String Name {
            get { return this.Retrieve(x => x.Name); }
            set { this.Store(x => x.Name, value); }
        }

        [Required]
        public Decimal Rate {
            get { return this.Retrieve(x => x.Rate); }
            set { this.Store(x => x.Rate, value); }
        }

        public string Title {
            get { return this.Name; }
        }
    }
}