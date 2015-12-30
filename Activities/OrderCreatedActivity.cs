using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace OShop.Activities {
    [OrchardFeature("OShop.Orders.Workflows")]
    public class OrderCreatedActivity : OrderActivity {
        public const string EventName = "OrderCreated";

        public override string Name {
            get { return EventName; }
        }

        public override LocalizedString Description {
            get { return T("An order is created."); }
        }
    }
}