using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingProviderPartHandler : ContentHandler {
        private readonly IShippingService _shippingService;

        public ShippingProviderPartHandler(
            IRepository<ShippingProviderPartRecord> repository,
            IShippingService shippingService) {

            _shippingService = shippingService;

            Filters.Add(StorageFilter.For(repository));

            OnInitializing<ShippingProviderPart>((context, part) => {
                part._options.Loader(value => value = new List<ShippingOptionRecord>());
            });

            OnCreated<ShippingProviderPart>((context, part) => {
                part._options.Loader(value => value = _shippingService.GetOptions(part));
            });

            OnLoaded<ShippingProviderPart>((context, part) => {
                part._options.Loader(value => value = _shippingService.GetOptions(part));
            });

            OnRemoved<ShippingProviderPart>((context, part) => {
                foreach (var option in part.Options) {
                    _shippingService.DeleteOption(option);
                }
            });
        }

    }
}