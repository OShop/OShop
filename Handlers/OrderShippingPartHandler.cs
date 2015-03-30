using Newtonsoft.Json;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;
using System.Collections.Generic;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Shipping")]
    public class OrderShippingPartHandler : ContentHandler {
        private IShippingService _shippingService;

        public OrderShippingPartHandler(
            IShippingService shippingService,
            IRepository<OrderShippingPartRecord> repository
            ) {
            _shippingService = shippingService;

            Filters.Add(StorageFilter.For(repository));

            OnActivated<OrderShippingPart>((context, part) => {
                part._provider.Loader(provider => _shippingService.GetProvider(part.ProviderId, part.ProviderVersionId));
                part._provider.Setter(provider => {
                    part.ProviderId = provider.Id;
                    part.ProviderVersionId = provider.ContentItem.Version;
                    return provider;
                });
            });
        }
    }
}