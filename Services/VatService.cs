using Orchard.Data;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    public class VatService : IVatService {
        private readonly IRepository<VatRecord> _vatRepository;

        public VatService(
            IRepository<VatRecord> vatRepository
            ) {
                _vatRepository = vatRepository;
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
            _vatRepository.Delete(record);
        }

        public VatRecord GetVat(int Id) {
            return _vatRepository.Get(Id);
        }

        public IEnumerable<VatRecord> ListVats() {
            return _vatRepository.Table.ToList();
        }


    }
}