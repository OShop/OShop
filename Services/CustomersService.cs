using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.ShoppingCart")]
    public class CustomersService : ICustomersService {
        private readonly IContentManager _contentManager;
        private readonly IClock _clock;

        public CustomersService(
            IContentManager contentManager,
            IOrchardServices services,
            IClock clock
            ) {
            _contentManager = contentManager;
            _clock = clock;
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        public Models.CustomerPart GetCustomer() {
            throw new NotImplementedException();
        }

        public Models.CustomerPart GetCustomer(int UserId) {
            throw new NotImplementedException();
        }
    }
}