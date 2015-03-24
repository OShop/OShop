using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;

namespace OShop.Services {
    public interface IVatService : IDependency {
        VatRatePart GetVatRate(Int32 Id, Int32? VersionRecordId = null);
        IEnumerable<VatRatePart> ListVatRates();
    }
}
