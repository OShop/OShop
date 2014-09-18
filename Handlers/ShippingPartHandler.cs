using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingPartHandler : ContentHandler {
        public ShippingPartHandler(IRepository<ShippingPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}