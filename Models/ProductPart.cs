using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using OShop.Models;
using System.ComponentModel.DataAnnotations;

namespace OShop.Models {
    public class ProductPart : ContentPart<ProductPartRecord>, IShopItem {
        public const string PartItemType = "Product";

        [Required]
        public decimal UnitPrice {
            get { return this.Retrieve(x => x.UnitPrice); }
            set { this.Store(x => x.UnitPrice, value); }
        }

        public string SKU {
            get { return this.Retrieve(x => x.SKU); }
            set { this.Store(x => x.SKU, value); }
        }

        public IContent Content {
            get { return this.As<IContent>(); }
        }

        public string Designation {
            get { return this.As<ITitleAspect>().Title; }
        }

        public string Description {
            get { return string.Empty; }
        }

        public string ItemType {
            get { return PartItemType; }
        }

        public decimal GetUnitPrice(int Quantity = 1) {
            return this.UnitPrice;
        }

        public decimal Price {
            get { return GetUnitPrice(); }
        }
    }
}