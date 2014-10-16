using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerPartHandler : ContentHandler {
        public CustomerPartHandler(IRepository<CustomerPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}