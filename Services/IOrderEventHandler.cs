using Orchard.ContentManagement;
using Orchard.Events;

namespace OShop.Services {
    public interface IOrderEventHandler : IEventHandler {
        void OrderCreated(IContent order);
        void OrderCompleted(IContent order);
        void OrderCanceled(IContent order);
    }
}
