using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingProviderPartHandler : ContentHandler {
        public ShippingProviderPartHandler(IRepository<ShippingProviderPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}