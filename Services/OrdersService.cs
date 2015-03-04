using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Environment.Extensions;
using Orchard.Security;
using Orchard.Services;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.Orders")]
    public class OrdersService : IOrdersService {
        private readonly IContentManager _contentManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IClock _clock;

        public OrdersService(
            IContentManager contentManager,
            IAuthenticationService authenticationService,
            IClock clock
            ) {
            _contentManager = contentManager;
            _authenticationService = authenticationService;
            _clock = clock;
        }

        public string BuildOrderReference() {
            String dateStr = _clock.UtcNow.ToLocalTime().ToString("yyyyMMddHHmmss");
            String newRef;
            Random rnd = new Random();
            do {
                newRef = dateStr + (char)rnd.Next(65, 91) + (char)rnd.Next(65, 91);
            }
            while (GetOrderByReference(newRef) != null);
            return newRef;
        }

        public OrderPart GetOrderByReference(string Reference) {
            return _contentManager.Query<OrderPart, OrderPartRecord>()
                .Where(o => o.Reference == Reference).Slice(1)
                .FirstOrDefault();
        }

        public IEnumerable<OrderPart> GetMyOrders() {
            var user = _authenticationService.GetAuthenticatedUser();

            if (user == null) {
                return new List<OrderPart>();
            }

            return GetOrdersByOwner(user.Id);
        }

        public IEnumerable<OrderPart> GetOrdersByOwner(int UserId) {
            return _contentManager.Query<CommonPart, CommonPartRecord>()
                .Where(c => c.OwnerId == UserId)
                .ForPart<OrderPart>()
                .List();
        }

    }
}