using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerAddressPartHandler : ContentHandler {
        public CustomerAddressPartHandler(IRepository<CustomerAddressPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}