using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace OShop.Models {
    public class OfflinePaymentSettingsPart : ContentPart {
        public readonly LazyField<ContentItem> _content = new LazyField<ContentItem>();

        public ContentItem Content
        {
            get { return _content.Value; }
            set
            {
                _content.Value = value;
                ContentItemId = value == null ? (int?)null : value.Record.Id;
            }
        }

        public int? ContentItemId
        {
            get { return this.Retrieve(x => x.ContentItemId); }
            set { this.Store(x => x.ContentItemId, value); }
        }
    }
}