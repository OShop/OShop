using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Payment")]
    public class PaymentPartHandler : ContentHandler {
        public PaymentPartHandler(IRepository<PaymentPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}