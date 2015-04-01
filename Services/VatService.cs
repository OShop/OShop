using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.VAT")]
    public class VatService : IVatService {
        private readonly IContentManager _contentManager;

        public VatService(
            IContentManager contentManager
            ) {
            _contentManager = contentManager;
        }


        public VatRatePart GetVatRate(int Id) {
            return _contentManager.Get<VatRatePart>(Id, VersionOptions.Published);
        }

        public IEnumerable<VatRatePart> ListVatRates() {
            return _contentManager.Query<VatRatePart>(VersionOptions.Published).List();
        }
    }
}