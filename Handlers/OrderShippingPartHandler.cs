using Newtonsoft.Json;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using System.Collections.Generic;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Shipping")]
    public class OrderShippingPartHandler : ContentHandler {
        public OrderShippingPartHandler(IRepository<OrderShippingPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));

        }
    }
}