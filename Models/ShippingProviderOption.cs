
namespace OShop.Models {
    public class ShippingProviderOption {
        public ShippingProviderOption(ShippingProviderPart provider, ShippingOptionRecord option) {
            this.Provider = provider;
            this.Option = option;
        }

        public ShippingProviderPart Provider;
        public ShippingOptionRecord Option;
    }
}