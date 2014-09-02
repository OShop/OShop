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
        private readonly IRepository<VatRecord> _vatRepository;
        private readonly IContentManager _contentManager;

        public VatService(
            IRepository<VatRecord> vatRepository,
            IContentManager contentManager
            ) {
            _vatRepository = vatRepository;
            _contentManager = contentManager;
        }

        public void AddVat(VatRecord record) {
            _vatRepository.Create(record);
        }

        public void UpdateVat(VatRecord record) {
            _vatRepository.Update(record);
        }

        public void DeleteVat(int Id) {
            DeleteVat(_vatRepository.Get(Id));
        }

        public void DeleteVat(VatRecord record) {
            if (record != null) {
                // Remove deleted VatRecord references from ProductParts
                var query = _contentManager.Query<ProductPart, ProductPartRecord>(VersionOptions.AllVersions)
                    .Where(p => p.VatRecord.Id == record.Id);

                foreach (var product in query.List()) {
                    product.VAT = null;
                }

                // Delete VatRecord
                _vatRepository.Delete(record);
            }
        }

        public VatRecord GetVat(int Id) {
            return _vatRepository.Get(Id);
        }

        public IEnumerable<VatRecord> ListVats() {
            return _vatRepository.Table.ToList();
        }


    }
}