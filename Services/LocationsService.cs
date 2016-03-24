using Orchard;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Tokens;
using System.Xml.Linq;

namespace OShop.Services {
    [OrchardFeature("OShop.Locations")]
    public class LocationsService : ILocationsService {
        private readonly IRepository<LocationsCountryRecord> _countryRepository;
        private readonly IRepository<LocationsStateRecord> _stateRepository;
        private readonly ITokenizer _tokenizer;

        public LocationsService(
            IRepository<LocationsCountryRecord> countryRepository,
            IRepository<LocationsStateRecord> stateRepository,
            ITokenizer tokenizer,
            IOrchardServices services
            ) {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _tokenizer = tokenizer;
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        public void AddCountry(LocationsCountryRecord record) {
            _countryRepository.Create(record);
        }

        public void UpdateCountry(LocationsCountryRecord record) {
            _countryRepository.Update(record);
        }

        public void DeleteCountry(int Id) {
            DeleteCountry(GetCountry(Id));
        }

        public void DeleteCountry(LocationsCountryRecord record) {
            foreach(var state in _stateRepository.Fetch(state => state.LocationsCountryRecord == record)) {
                _stateRepository.Delete(state);
            }
            _countryRepository.Delete(record);
        }

        public LocationsCountryRecord GetCountry(int Id) {
            return _countryRepository.Get(Id);
        }

        public IEnumerable<LocationsCountryRecord> GetCountries() {
            return _countryRepository.Table.OrderBy(c => c.Name);
        }

        public IEnumerable<LocationsCountryRecord> GetEnabledCountries() {
            return _countryRepository.Fetch(c => c.Enabled).OrderBy(c => c.Name);
        }

        public int GetDefaultCountryId() {
            var settings = Services.WorkContext.CurrentSite.As<OShopSettingsPart>();
            if (settings != null) {
                return settings.DefaultCountryId;
            }
            else {
                return 0;
            }
        }

        public LocationsCountryRecord GetDefaultCountry() {
            return _countryRepository.Get(GetDefaultCountryId());
        }

        public void SetDefaulCountryId(int Id) {
            var settings = Services.WorkContext.CurrentSite.As<OShopSettingsPart>();
            if (settings != null) {
                settings.DefaultCountryId = Id;
            }
        }

        public void SetDefaulCountry(LocationsCountryRecord record) {
            if (record != null) {
                SetDefaulCountryId(record.Id);
            }
        }

        public void AddState(LocationsStateRecord record) {
            _stateRepository.Create(record);
        }

        public void UpdateState(LocationsStateRecord record) {
            _stateRepository.Update(record);
        }

        public void DeleteState(int Id) {
            DeleteState(GetState(Id));
        }

        public void DeleteState(LocationsStateRecord record) {
            _stateRepository.Delete(record);
        }

        public LocationsStateRecord GetState(int Id) {
            return _stateRepository.Get(Id);
        }

        public IEnumerable<LocationsStateRecord> GetStates() {
            return _stateRepository.Table.OrderBy(s => s.Name);
        }

        public IEnumerable<LocationsStateRecord> GetStates(int CountryId) {
            return _stateRepository.Fetch(s => s.LocationsCountryRecord.Id == CountryId).OrderBy(s => s.Name);
        }

        public IEnumerable<LocationsStateRecord> GetEnabledStates(int CountryId) {
            return _stateRepository.Fetch(s => s.Enabled && s.LocationsCountryRecord.Enabled && s.LocationsCountryRecord.Id == CountryId).OrderBy(s => s.Name);
        }

        public string FormatAddress(IOrderAddress address) {
            if (address.Country == null || String.IsNullOrWhiteSpace(address.Country.AddressFormat)) {
                // TODO : Provide default format
                return "";
            }
            else {
                return _tokenizer.Replace(
                    address.Country.AddressFormat,
                    new Dictionary<string, object> { { "OrderAddress", address } }
                ).Trim(new char[]{' ', '-', '\r', '\n'});
            }
        }

        public void Import(XDocument ImportedLocations) {
            foreach (var xCountry in ImportedLocations.Root.Elements("country")) {
                var countryIsoCode = xCountry.Attribute("iso_code").Value;
                var country = _countryRepository.Get(c => c.IsoCode == countryIsoCode);
                if(country == null) {
                    country = new LocationsCountryRecord() {
                        IsoCode = countryIsoCode,
                        Name = xCountry.Element("name").Value,
                        AddressFormat = xCountry.Element("address_format").Value
                    };
                    _countryRepository.Create(country);
                }
                else {
                    country.Name = xCountry.Element("name").Value;
                    country.AddressFormat = xCountry.Element("address_format").Value;
                }
                foreach(var xState in xCountry.Element("states").Elements("state")) {
                    var stateIsoCode = xState.Attribute("iso_code").Value;
                    var state = _stateRepository.Get(s => s.IsoCode == stateIsoCode && s.LocationsCountryRecord.Id == country.Id);
                    if(state == null) {
                        state = new LocationsStateRecord() {
                            IsoCode = stateIsoCode,
                            Name = xState.Element("name").Value,
                            LocationsCountryRecord = country
                        };
                        _stateRepository.Create(state);
                    }
                    else {
                        state.Name = xState.Element("name").Value;
                        _stateRepository.Update(state);
                    }
                }
            }
        }
    }
}