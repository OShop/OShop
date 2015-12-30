using Orchard.ContentManagement;
using Orchard.Events;
using OShop.Models;

namespace OShop.Events {
    public interface IOrderEventHandler : IEventHandler {
        void OrderCreated(IContent order);
        void OrderCompleted(IContent order);
        void OrderCanceled(IContent order);
        void OrderDetailCreated(IContent order, OrderDetailRecord createdDetail);
        void OrderDetailUpdated(IContent order, OrderDetailRecord originalDetail, OrderDetailRecord updatedDetail);
        void OrderDetailDeleted(IContent order, OrderDetailRecord deletedDetail);
    }
}
