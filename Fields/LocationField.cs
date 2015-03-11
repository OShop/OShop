using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System;

namespace OShop.Fields {
    [OrchardFeature("OShop.Locations")]
    public class LocationField : ContentField {
        public Int32 CountryId {
            get {
                return Storage.Get<Int32>("CountryId");
            }
            set {
                Storage.Set("CountryId", value);
            }
        }

        public Int32 StateId {
            get {
                return Storage.Get<Int32>("StateId");
            }
            set {
                Storage.Set("StateId", value);
            }
        }
    }
}