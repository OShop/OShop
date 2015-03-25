using Orchard.Environment.Extensions;
using Orchard.Localization;
using System.Web.Routing;

namespace OShop.Services {
    [OrchardFeature("OShop.Checkout")]
    public class CheckoutService : ICheckoutProvider {

        public CheckoutService() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public int Priority {
            get { return 100; }
        }

        public string Name {
            get { return "Checkout"; }
        }

        public LocalizedString Label {
            get { return T("Checkout"); }
        }

        public RouteValueDictionary CheckoutRoute {
            get { return new RouteValueDictionary(
                new {
                    action = "Index",
                    controller = "Checkout",
                    area = "OShop"
                });
            }
        }
    }
}