using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingService : IShippingService {
        private readonly IRepository<ShippingZoneRecord> _zoneRepository;
        private readonly IRepository<LocationsCountryRecord> _countryRepository;
        private readonly IRepository<LocationsStateRecord> _stateRepository;
        private readonly IRepository<ShippingOptionRecord> _optionRepository;

        public ShippingService(
            IRepository<ShippingZoneRecord> zoneRepository,
            IRepository<LocationsCountryRecord> countryRepository,
            IRepository<LocationsStateRecord> stateRepository,
            IRepository<ShippingOptionRecord> optionRepository) {
            _zoneRepository = zoneRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _optionRepository = optionRepository;
        }

        #region Shipping zones
        public void CreateZone(ShippingZoneRecord record) {
            _zoneRepository.Create(record);
        }

        public void UpdateZone(ShippingZoneRecord record) {
            _zoneRepository.Update(record);
        }

        public void DeleteZone(int ZoneId) {
            DeleteZone(GetZone(ZoneId));
        }

        public void DeleteZone(ShippingZoneRecord record) {
            if (record != null) {
                foreach (var country in _countryRepository.Fetch(c => c.ShippingZoneRecord != null && c.ShippingZoneRecord.Id == record.Id)) {
                    country.ShippingZoneRecord = null;
                };
                foreach (var state in _stateRepository.Fetch(s => s.ShippingZoneRecord != null && s.ShippingZoneRecord.Id == record.Id)) {
                    state.ShippingZoneRecord = null;
                }
                foreach (var option in _optionRepository.Fetch(o => o.ShippingZoneRecord != null && o.ShippingZoneRecord.Id == record.Id)) {
                    option.ShippingZoneRecord = null;
                }

                _zoneRepository.Delete(record);
            }
        }

        public ShippingZoneRecord GetZone(int Id) {
            return _zoneRepository.Get(Id);
        }

        public IEnumerable<ShippingZoneRecord> GetZones() {
            return _zoneRepository.Table.OrderBy(z => z.Name);
        }

        public IEnumerable<ShippingZoneRecord> GetEnabledZones() {
            return _zoneRepository.Fetch(z => z.Enabled).OrderBy(z => z.Name);
        }
        
        #endregion

        #region Shipping options
        public void CreateOption(ShippingOptionRecord record) {
            _optionRepository.Create(record);
        }

        public void UpdateOption(ShippingOptionRecord record) {
            _optionRepository.Update(record);
        }

        public void DeleteOption(int OptionId) {
            DeleteOption(GetOption(OptionId));
        }

        public void DeleteOption(ShippingOptionRecord record) {
            _optionRepository.Delete(record);
        }

        public ShippingOptionRecord GetOption(int Id) {
            return _optionRepository.Get(Id);
        }

        public IEnumerable<ShippingOptionRecord> GetOptions(ShippingProviderPart part) {
            return _optionRepository.Fetch(o => o.ShippingProviderPartRecord.Id == part.Id);
        }
        
        #endregion


    }
}