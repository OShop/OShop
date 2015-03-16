using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;
using OShop.Permissions;

namespace OShop.Navigation {
    [OrchardFeature("OShop.Customers")]
    public class CustomersAdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public CustomersAdminMenu() {
            T = NullLocalizer.Instance;
        }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder
                .Add(menu => menu
                    .Caption(T("OShop"))
                    .Add(subMenu => subMenu
                        .Caption(T("Customers"))
                        .Position("7")
                        .Permission(CustomersPermissions.ViewCustomerAccounts)
                        .Action("Index", "CustomersAdmin", new { area = "OShop" })
                    )
                );
        }
    }
}