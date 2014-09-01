using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;
using OShop.Permissions;

namespace OShop.Navigation {
    [OrchardFeature("OShop.Products")]
    public class ProductsAdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public ProductsAdminMenu() {
            T = NullLocalizer.Instance;
        }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder
                .Add(menu => menu
                    .Caption(T("OShop"))
                    .Add(subMenu => subMenu
                        .Caption(T("Products"))
                        .Position("1")
                        .Action("List", "ProductsAdmin", new { area = "OShop" })
                        .Permission(OShopPermissions.AccessShopPanel)
                    )
                );
        }
    }
}