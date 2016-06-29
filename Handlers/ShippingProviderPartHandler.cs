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
            IShippingService shippingService) {

            _shippingService = shippingService;

            OnActivated<ShippingProviderPart>((context, part) => {
                part._options.Loader(() => _shippingService.GetOptions(part));
            });

            OnRemoved<ShippingProviderPart>((context, part) => {
                foreach (var option in part.Options) {
                    _shippingService.DeleteOption(option);
                }
            });
        }

    }
}