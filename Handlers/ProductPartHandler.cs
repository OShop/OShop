using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Products")]
    public class ProductPartHandler : ContentHandler {
        public ProductPartHandler(IRepository<ProductPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}