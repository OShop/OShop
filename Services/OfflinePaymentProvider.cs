using Orchard.Environment.Extensions;
using Orchard.Localization;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace OShop.Services {
    [OrchardFeature("OShop.OfflinePayment")]
    public class OfflinePaymentProvider : IPaymentProvider {

        public OfflinePaymentProvider() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public int Priority {
            get { return 100; }
        }

        public string Name {
            get { return "Offline"; }
        }

        public LocalizedString Label {
            get { return T("Offline payment"); }
        }

        public LocalizedString Description {
            get { return T("Pay by check or bank wire"); }
        }

        public RouteValueDictionary GetPaymentRoute(PaymentPart Part) {
            return null;
        }
    }
}