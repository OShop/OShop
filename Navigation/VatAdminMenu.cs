using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;
using OShop.Permissions;

namespace OShop.Navigation {
    [OrchardFeature("OShop.VAT")]
    public class VatAdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public VatAdminMenu() {
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
                            .Caption(T("VAT"))
                            .Position("5")
                            .Action("Index", "VatAdmin", new { area = "OShop" })
                            .Permission(OShopPermissions.ManageShopSettings)
                            .LocalNav()
                        )
                        .Add(tab => tab
                            .Caption(T("VAT rates"))
                            .Position("5.1")
                            .Action("Rates", "VatAdmin", new { area = "OShop" })
                            .Permission(OShopPermissions.ManageShopSettings)
                            .LocalNav()
                        )
                    )
                );
        }
    }
}