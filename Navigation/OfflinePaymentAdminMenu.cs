using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;
using OShop.Permissions;

namespace OShop.Navigation {
    [OrchardFeature("OShop.OfflinePayment")]
    public class OfflinePaymentAdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public OfflinePaymentAdminMenu() {
            T = NullLocalizer.Instance;
        }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder
                .Add(menu => menu
                    .Caption(T("OShop"))
                    .Add(subMenu => subMenu
                        .Caption(T("Settings"))
                        .Add(tab => tab
                            .Caption(T("Offline Payment"))
                            .Position("7")
                            .Action("Settings", "OfflinePayment", new { area = "OShop" })
                            .Permission(OShopPermissions.ManageShopSettings)
                            .LocalNav()
                        )
                    )
                );
        }
    }
}