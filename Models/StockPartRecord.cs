using Orchard.ContentManagement.Records;

namespace OShop.Models {
    public class StockPartRecord : ContentPartRecord {
        public virtual bool EnableStockMgmt { get; set; }
        public virtual int InStockQty { get; set; }
        public virtual int AlertQty { get; set; }
        public virtual bool AllowOutOfStock { get; set; }
    }
}