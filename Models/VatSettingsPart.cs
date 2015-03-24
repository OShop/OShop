using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class VatSettingsPart : ContentPart {
        public bool ShowVatPriceInFrontEnd {
            get { return this.Retrieve(x => x.ShowVatPriceInFrontEnd, true); }
            set { this.Store(x => x.ShowVatPriceInFrontEnd, value); }
        }

        public bool ShowVatPriceInAdmin {
            get { return this.Retrieve(x => x.ShowVatPriceInAdmin, false); }
            set { this.Store(x => x.ShowVatPriceInAdmin, value); }
        }
    }
}