using Orchard.Localization;
using Orchard.UI.Navigation;
using OShop.Permissions;

namespace OShop.Navigation {
    public class AdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public AdminMenu() {
            T = NullLocalizer.Instance;
        }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder
                .AddImageSet("oshop")
                .Add(menu => menu
                    .Caption(T("OShop"))
                    .Position("4")
                    .LinkToFirstChild(false)
                    .Permission(OShopPermissions.AccessShopPanel)
                    .Add(subMenu => subMenu
                        .Caption(T("Settings"))
                        .Position("20")
                        .Action("Index", "Settings", new { area = "OShop"})
                        .Permission(OShopPermissions.ManageShopSettings)
                        .Add(tab => tab
                            .Caption(T("General"))
                            .Position("1")
                            .Action("Index", "Settings", new { area = "OShop" })
                            .Permission(OShopPermissions.ManageShopSettings)
                            .LocalNav()
                        )
                    )
                );
        }
    }
}