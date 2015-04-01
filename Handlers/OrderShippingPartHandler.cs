﻿using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Shipping")]
    public class OrderShippingPartHandler : ContentHandler {
        public OrderShippingPartHandler(
            IRepository<OrderShippingPartRecord> repository
            ) {
            Filters.Add(StorageFilter.For(repository));

            OnActivated<OrderShippingPart>((context, part) => {
                part._shippingDetails.Loader(shippingDetails => {
                    var orderPart = context.ContentItem.As<OrderPart>();
                    if (orderPart != null) {
                        return orderPart.Details.Where(d => d.DetailType == "Shipping");
                    }
                    else {
                        return new List<OrderDetail>();
                    }
                });
            });
        }
    }
}