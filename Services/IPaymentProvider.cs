using Orchard;
using Orchard.Localization;
using OShop.Models;
using System.Web.Routing;

namespace OShop.Services {
    public interface IPaymentProvider : IDependency {
        int Priority { get; }
        string Name { get; }
        LocalizedString Label { get; }
        LocalizedString Description { get; }
        RouteValueDictionary GetPaymentRoute(PaymentPart Part);
    }
}
