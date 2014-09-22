using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;
using OShop.Permissions;

namespace OShop.Navigation {
    [OrchardFeature("OShop.Locations")]
    public class LocationsAdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public LocationsAdminMenu() {
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
                            .Caption(T("Countries"))
                            .Position("3")
                            .Action("Index", "LocationsAdmin", new { area = "OShop" })
                            .Permission(OShopPermissions.ManageShopSettings)
                            .LocalNav()
                        )
                        .Add(tab => tab
                            .Caption(T("States"))
                            .Position("4")
                            .Action("States", "LocationsAdmin", new { area = "OShop" })
                            .Permission(OShopPermissions.ManageShopSettings)
                            .LocalNav()
                        )
                    )
                );
        }
    }
}