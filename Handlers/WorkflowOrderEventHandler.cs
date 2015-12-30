using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Workflows.Services;
using OShop.Activities;
using OShop.Events;
using OShop.Models;
using System.Collections.Generic;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Orders.Workflows")]
    public class WorkflowOrderEventHandler : IOrderEventHandler {
        private readonly IWorkflowManager _workflowManager;

        public WorkflowOrderEventHandler(IWorkflowManager workflowManager) {
            _workflowManager = workflowManager;
        }

        public void OrderCanceled(IContent order) {
            _workflowManager.TriggerEvent(OrderCanceledActivity.EventName, order, () => new Dictionary<string, object> { { "Content", order } });
        }

        public void OrderCompleted(IContent order) {
            _workflowManager.TriggerEvent(OrderCompletedActivity.EventName, order, () => new Dictionary<string, object> { { "Content", order } });
        }

        public void OrderCreated(IContent order) {
            _workflowManager.TriggerEvent(OrderCreatedActivity.EventName, order, () => new Dictionary<string, object> { { "Content", order } });
        }

        public void OrderDetailCreated(IContent order, OrderDetailRecord createdDetail) {
        }

        public void OrderDetailDeleted(IContent order, OrderDetailRecord deletedDetail) {
        }

        public void OrderDetailUpdated(IContent order, OrderDetailRecord originalDetail, OrderDetailRecord updatedDetail) {
        }
    }
}