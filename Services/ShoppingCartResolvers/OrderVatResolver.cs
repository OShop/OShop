using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Helpers;
using OShop.Models;
using System.Linq;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.VAT")]
    public class OrderVatResolver : IOrderBuilder {
        private readonly IContentManager _contentManager;

        public OrderVatResolver(
            IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public int Priority {
            get { return 50; }
        }

        public void BuildOrder(IShoppingCartService ShoppingCartService, IContent Order) {
            // Attach VAT infos to OrderDetails
            var orderPart = Order.As<OrderPart>();
            if (orderPart != null) {
                var vatParts = _contentManager.GetMany<VatPart>(orderPart.Details.Select(d => d.ContentId), VersionOptions.Published, QueryHints.Empty);
                foreach(var vatDetailPair in orderPart.Details.Join(vatParts, od => od.ContentId, vat => vat.Id, (od, vat) => new { Detail = od, Vat = vat})) {
                    if (vatDetailPair.Vat.VatRate != null) {
                        vatDetailPair.Detail.SetAttribute("VAT", new Tax(vatDetailPair.Vat.VatRate));
                    }
                }
            }
        }
    }
}