using Orchard.Localization;
using Orchard.UI.Navigation;
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
            builder.AddImageSet("oshop")
                .Add(T("OShop"), "6",
                    menu => menu.Add(T("OShop"), "0", item => item.Action("Index", "Admin", new { area = "Orchard.MediaLibrary" })
                        .Permission(OShopPermissions.AccessShopPanel)));
        }
    }
}