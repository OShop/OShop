using Orchard.ContentManagement;
using System.ComponentModel.DataAnnotations;
using System;

namespace OShop.Models {
    public class StockPart : ContentPart<StockPartRecord>, IStock {
        [Required]
        public bool EnableStockMgmt {
            get { return this.Retrieve(x => x.EnableStockMgmt); }
            set { this.Store(x => x.EnableStockMgmt, value); }
        }

        [Required]
        public int InStockQty {
            get { return this.Retrieve(x => x.InStockQty); }
            set { this.Store(x => x.InStockQty, value); }
        }

        public int InOrderQty {
            get { return this.Retrieve(x => x.InOrderQty); }
            set { this.Store(x => x.InOrderQty, value); }
        }

        [Required]
        public int AlertQty {
            get { return this.Retrieve(x => x.AlertQty); }
            set { this.Store(x => x.AlertQty, value); }
        }

        [Required]
        public bool AllowOutOfStock {
            get { return this.Retrieve(x => x.AllowOutOfStock); }
            set { this.Store(x => x.AllowOutOfStock, value); }
        }

        public int? AvailableQty {
            get { return EnableStockMgmt ? (int?)(InStockQty - InOrderQty) : null; }
        }

        public int? MaxOrderQty {
            get { return AllowOutOfStock ? null : AvailableQty; }
        }
    }
}