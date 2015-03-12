using Newtonsoft.Json;
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
    [OrchardFeature("OShop.Orders")]
    public class OrderPartHandler : ContentHandler {

        public OrderPartHandler(
            IRepository<OrderPartRecord> repository,
            IOrdersService ordersService
            ) {

            Filters.Add(StorageFilter.For(repository));

            OnCreating<OrderPart>((context, part) => {
                if (String.IsNullOrWhiteSpace(part.Reference)) {
                    part.Reference = ordersService.BuildOrderReference();
                }
            });
        }

    }
}