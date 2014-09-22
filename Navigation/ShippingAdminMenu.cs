using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;
using OShop.Permissions;

namespace OShop.Navigation {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingAdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public ShippingAdminMenu() {
            T = NullLocalizer.Instance;
        }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder
                .Add(menu => menu
                    .Caption(T("OShop"))
                    .Add(subMenu => subMenu
                        .Caption(T("Shipping"))
                        .Position("8")
                        .Permission(OShopPermissions.ManageShopSettings)
                        .Add(tab => tab
                            .Caption(T("Zones"))
                            .Position("5")
                            .Action("Index", "ShippingZonesAdmin", new { area = "OShop" })
                            .Permission(OShopPermissions.ManageShopSettings)
                            .LocalNav()
                            )
                    )
                );
        }
    }
}