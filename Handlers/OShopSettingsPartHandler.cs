using Orchard.ContentManagement.Handlers;
using OShop.Models;

namespace OShop.Handlers {
    public class OShopSettingsPartHandler : ContentHandler {
        public OShopSettingsPartHandler() {
            Filters.Add(new ActivatingFilter<OShopSettingsPart>("Site"));
        }
    }
}