using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Products")]
    public class ProductResolver : IShoppingCartBuilder, IOrderBuilder {
        private readonly IContentManager _contentManager;

        public ProductResolver(
            IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public Int32 Priority {
            get { return 100; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ref ShoppingCart Cart) {
            var cartRecords = ShoppingCartService.ListItems();
            var products = _contentManager.GetMany<ProductPart>(
                cartRecords.Where(cr => cr.ItemType == ProductPart.PartItemType).Select(cr => cr.ItemId),
                VersionOptions.Published,
                QueryHints.Empty);

            foreach (var cartRecord in cartRecords.Where(cr => cr.ItemType == ProductPart.PartItemType)) {
                var product = products.Where(p => p.Id == cartRecord.ItemId).FirstOrDefault();

                if (product != null) {
                    Cart.Items.Add(new ShoppingCartItem {
                        Id = cartRecord.Id,
                        Item = product,
                        Quantity = cartRecord.Quantity
                    });
                }
            }
        }

        public void BuildOrder(IShoppingCartService ShoppingCartService, ref IContent Order) {
            var orderPart = Order.As<OrderPart>();
            if (orderPart != null) {
                var cartRecords = ShoppingCartService.ListItems();
                var products = _contentManager.GetMany<ProductPart>(
                    cartRecords.Where(cr => cr.ItemType == ProductPart.PartItemType).Select(cr => cr.ItemId),
                    VersionOptions.Published,
                    QueryHints.Empty);
                var orderItems = orderPart.Items ?? new List<OrderItem>();
                foreach (var cartRecord in cartRecords.Where(cr => cr.ItemType == ProductPart.PartItemType)) {
                    var product = products.Where(p => p.Id == cartRecord.ItemId).FirstOrDefault();

                    if (product != null) {
                        orderItems.Add(new OrderItem {
                            SKU = product.SKU,
                            ContentId = product.Content.Id,
                            Designation = product.Designation,
                            Description = product.Description,
                            UnitPrice = product.UnitPrice,
                            Quantity = cartRecord.Quantity,
                            VatId = product.VAT != null ? product.VAT.Id : 0
                        });
                    }
                }
                orderPart.Items = orderItems;
            }
        }

    }
}