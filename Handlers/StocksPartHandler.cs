using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Stocks")]
    public class StocksPartHandler : ContentHandler {
        public StocksPartHandler(IRepository<StockPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }

    }
}