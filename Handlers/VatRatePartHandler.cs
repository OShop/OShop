using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Handlers {
    [OrchardFeature("OShop.VAT")]
    public class VatRatePartHandler : ContentHandler {
        public VatRatePartHandler(IRepository<VatRatePartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}