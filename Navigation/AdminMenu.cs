using Orchard.Localization;
using Orchard.UI.Navigation;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                        .Position("10")
                        .Action("Index", "Settings", new { area = "OShop"})
                        .Permission(OShopPermissions.ManageShopSettings)
                    )
                );
        }
    }
}