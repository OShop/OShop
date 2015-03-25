using Orchard;
using Orchard.Localization;
using System.Web.Routing;

namespace OShop.Services {
    public interface ICheckoutProvider : IDependency {
        int Priority { get; }
        string Name { get; }
        LocalizedString Label { get; }
        RouteValueDictionary CheckoutRoute { get; }
    }
}
