using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Handlers {
    [OrchardFeature("OShop.VAT")]
    public class VatSettingsPartHandler : ContentHandler {
        public VatSettingsPartHandler() {
            Filters.Add(new ActivatingFilter<VatSettingsPart>("Site"));
        }
    }
}