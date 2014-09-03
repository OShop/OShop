using Orchard.ContentManagement;
using Orchard.Core.Contents.ViewModels;
using System.Collections.Generic;
using System.Globalization;

namespace OShop.ViewModels {
    public class ProductListViewModel {
        public ProductListViewModel() {
            Options = new ContentOptions();
            Options.ContentsStatus = ContentsStatus.Published;
        }

        public string Id { get; set; }

        public string TypeName {
            get { return Id; }
        }

        public string TypeDisplayName { get; set; }
        public NumberFormatInfo NumberFormat { get; set; }
        public IList<ContentItem> Products { get; set; }
        public ContentOptions Options { get; set; }
        public dynamic Pager { get; set; }

        // Optional features
        public bool VatEnabled { get; set; }
    }
}