using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;
using OShop.Permissions;

namespace OShop.Navigation {
    [OrchardFeature("OShop.Orders")]
    public class OrdersAdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public OrdersAdminMenu() {
            T = NullLocalizer.Instance;
        }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder
                .Add(menu => menu
                    .Caption(T("OShop"))
                    .Add(subMenu => subMenu
                        .Caption(T("Orders"))
                        .Position("8")
                        .Permission(OrdersPermissions.ViewOrders)
                        .Action("Index", "OrdersAdmin", new { area = "OShop" })
                    )
                );
        }
    }
}