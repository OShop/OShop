using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Events;
using OShop.Models;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Stocks")]
    public class StocksOrderEventHandler : IDependency, IOrderEventHandler {
        private readonly IContentManager _contentManager;

        public StocksOrderEventHandler(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public void OrderCanceled(IContent order) {
        }

        public void OrderCompleted(IContent order) {
        }

        public void OrderCreated(IContent order) {
        }

        public void OrderDetailCreated(IContent order, OrderDetailRecord createdDetail) {
            var stockPart = _contentManager.Get(createdDetail.ContentId).As<StockPart>();
            var orderPart = order.As<OrderPart>();
            if(stockPart != null && stockPart.EnableStockMgmt && orderPart != null) {
                if(orderPart.OrderStatus == OrderStatus.Canceled) {
                    return;
                }
                else if(orderPart.OrderStatus < OrderStatus.Completed) {
                    stockPart.InOrderQty += createdDetail.Quantity;
                }
                else {
                    stockPart.InStockQty -= createdDetail.Quantity;
                }
            }
        }

        public void OrderDetailDeleted(IContent order, OrderDetailRecord deletedDetail) {
            var stockPart = _contentManager.Get(deletedDetail.ContentId).As<StockPart>();
            var orderPart = order.As<OrderPart>();
            if (stockPart != null && stockPart.EnableStockMgmt && orderPart != null) {
                if (orderPart.OrderStatus == OrderStatus.Canceled) {
                    return;
                }
                else if (orderPart.OrderStatus < OrderStatus.Completed) {
                    stockPart.InOrderQty -= deletedDetail.Quantity;
                }
                else {
                    stockPart.InStockQty += deletedDetail.Quantity;
                }
            }
        }

        public void OrderDetailUpdated(IContent order, OrderDetailRecord originalDetail, OrderDetailRecord updatedDetail) {
            if(originalDetail.ContentId == updatedDetail.ContentId) {
                var orderPart = order.As<OrderPart>();
                var stockPart = _contentManager.Get(updatedDetail.ContentId).As<StockPart>();
                if (stockPart != null && stockPart.EnableStockMgmt && orderPart != null) {
                    if(orderPart.OriginalStatus != orderPart.OrderStatus) {
                        // OrderStatus changed
                        if (orderPart.OriginalStatus == OrderStatus.Canceled) {
                            if(orderPart.OrderStatus < OrderStatus.Completed) {
                                stockPart.InOrderQty += updatedDetail.Quantity;
                            }
                            else {
                                stockPart.InStockQty -= updatedDetail.Quantity;
                            }
                        }
                        else if (orderPart.OriginalStatus < OrderStatus.Completed) {
                            if (orderPart.OrderStatus == OrderStatus.Canceled) {
                                stockPart.InOrderQty -= originalDetail.Quantity;
                            }
                            else if (orderPart.OrderStatus == OrderStatus.Completed) {
                                stockPart.InOrderQty -= originalDetail.Quantity;
                                stockPart.InStockQty -= updatedDetail.Quantity;
                            }
                            else {
                                stockPart.InOrderQty += updatedDetail.Quantity - originalDetail.Quantity;
                            }
                        }
                        else {
                            stockPart.InStockQty += originalDetail.Quantity;
                            if (orderPart.OrderStatus != OrderStatus.Canceled) {
                                stockPart.InOrderQty += updatedDetail.Quantity;
                            }
                        }
                    }
                    else {
                        // OrderStatus unchanged
                        if (originalDetail.Quantity != updatedDetail.Quantity) {
                            stockPart.InOrderQty += updatedDetail.Quantity - originalDetail.Quantity;
                        }
                    }
                }
            }
            else {
                OrderDetailDeleted(order, originalDetail);
                OrderDetailCreated(order, updatedDetail);
            }
        }
    }
}