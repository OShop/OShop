
using Orchard.ContentManagement;
namespace OShop.Models {
    public class ShippingProviderOption : IPrice {
        public ShippingProviderOption(ShippingProviderPart provider, ShippingOptionRecord option) {
            this.Provider = provider;
            this.Option = option;
        }

        public ShippingProviderPart Provider;
        public ShippingOptionRecord Option;

        public decimal Price {
            get { return Option.Price; }
        }

        public ContentItem ContentItem {
            get { return Provider.ContentItem; }
        }

        public int Id {
            get { return Provider.ContentItem.Id; }
        }
    }
}