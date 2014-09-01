using Orchard;
using OShop.Models;
using System.Collections.Generic;

namespace OShop.Services {
    public interface IVatService : IDependency {
        void AddVat(VatRecord record);
        void UpdateVat(VatRecord record);
        void DeleteVat(int Id);
        void DeleteVat(VatRecord record);
        VatRecord GetVat(int Id);
        IEnumerable<VatRecord> ListVats();
    }
}
