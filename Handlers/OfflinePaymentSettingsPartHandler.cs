using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using OShop.Models;

namespace OShop.Handlers {
    [OrchardFeature("OShop.OfflinePayment")]
    public class OfflinePaymentSettingsPartHandler : ContentHandler {
        private readonly IContentManager _contentManager;

        public OfflinePaymentSettingsPartHandler(IContentManager contentManager) {
            _contentManager = contentManager;
            Filters.Add(new ActivatingFilter<OfflinePaymentSettingsPart>("Site"));

            OnLoading<OfflinePaymentSettingsPart>((context, part) => part._content.Loader(p => {
                if (part.ContentItemId != null) {
                    return contentManager.Get(part.ContentItemId.Value);
                }

                return null;
            }));
        }
    }
}