using Orchard.ContentManagement;
using OShop.Models;
using System.ComponentModel.DataAnnotations;

namespace OShop.Models {
    public class ProductPart : ContentPart<ProductPartRecord> {
        [Required]
        public decimal UnitPrice {
            get { return this.Retrieve(x => x.UnitPrice); }
            set { this.Store(x => x.UnitPrice, value); }
        }

        public string SKU {
            get { return this.Retrieve(x => x.SKU); }
            set { this.Store(x => x.SKU, value); }
        }

        public VatRecord VAT {
            get { return Record.VatRecord; }
            set { Record.VatRecord = value; }
        }
    }
}