using Orchard;
using Orchard.ContentManagement;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Services {
    public interface IOrdersService : IDependency {
        void CreateOrder(IContent order);
        String BuildOrderReference();
        OrderPart GetOrderByReference(string Refrence);
        IEnumerable<OrderPart> GetMyOrders();
        IEnumerable<OrderPart> GetOrdersByOwner(Int32 UserId);
    }
}
