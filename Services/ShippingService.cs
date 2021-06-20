using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using System.Diagnostics;

namespace OShop.Services {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingService : IShippingService {
        private readonly IRepository<ShippingZoneRecord> _zoneRepository;
        private readonly IRepository<LocationsCountryRecord> _countryRepository;
        private readonly IRepository<LocationsStateRecord> _stateRepository;
        private readonly IRepository<ShippingOptionRecord> _optionRepository;
        private readonly IContentManager _contentManager;

        public ShippingService(
            IRepository<ShippingZoneRecord> zoneRepository,
            IRepository<LocationsCountryRecord> countryRepository,
            IRepository<LocationsStateRecord> stateRepository,
            IRepository<ShippingOptionRecord> optionRepository,
            IContentManager contentManager) {
            _zoneRepository = zoneRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _optionRepository = optionRepository;
            _contentManager = contentManager;
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

        public ShippingProviderPart GetProvider(int ProviderId, int? VersionRecordId = null) {
            return _contentManager.Get<ShippingProviderPart>(ProviderId, VersionRecordId.HasValue ? VersionOptions.VersionRecord(VersionRecordId.Value) : VersionOptions.Published);
        }

        public ShippingOptionRecord GetOption(int Id) {
            return _optionRepository.Get(Id);
        }

        public IEnumerable<ShippingOptionRecord> GetOptions(ShippingProviderPart part) {
            return _optionRepository.Fetch(o => o.ShippingProviderId == part.Id);
        }

        public IEnumerable<ShippingProviderOption> GetSuitableProviderOptions(ShippingZoneRecord Zone, IList<Tuple<int, IShippingInfo>> ShippingInfos, Decimal ItemsTotal = 0) {
            var options = _optionRepository
                .Fetch(o => o.ShippingZoneRecord == Zone && o.Enabled);

            List<ShippingProviderOption> suitableProviders = new List<ShippingProviderOption>();
            foreach (var optionGroup in options.GroupBy(o => o.ShippingProviderId)) {
                var provider = _contentManager.Get<ShippingProviderPart>(optionGroup.Key, VersionOptions.Published);
                if (provider == null) {
                    continue;
                }

                var option = GetSuitableOption(optionGroup, Zone, ShippingInfos, ItemsTotal);
                if (option != null) {
                    suitableProviders.Add(new ShippingProviderOption(provider, option));
                }
            }

            return suitableProviders;
        }

        #endregion

        private ShippingOptionRecord GetSuitableOption(IEnumerable<ShippingOptionRecord> options, ShippingZoneRecord Zone, IList<Tuple<int, IShippingInfo>> ShippingInfos, Decimal ItemsTotal) {
            if (Zone == null) {
                return null;
            }

            return options
                .OrderByDescending(o => o.Priority)
                .Where(o => MeetsContraints(o, ShippingInfos, ItemsTotal))
                .FirstOrDefault();
        }

        private bool MeetsContraints(ShippingOptionRecord option, IList<Tuple<int, IShippingInfo>> ShippingInfos, Decimal ItemsTotal) {
            foreach (var contraint in option.Contraints) {
                double propertyValue = EvalProperty(contraint.Property, ShippingInfos, ItemsTotal);
                switch (contraint.Operator) {
                    case ShippingContraintOperator.LessThan:
                        if (contraint.Value <= propertyValue)
                            return false;
                        break;
                    case ShippingContraintOperator.LessThanOrEqual:
                        if (contraint.Value < propertyValue)
                            return false;
                        break;
                    case ShippingContraintOperator.Equal:
                        if (contraint.Value != propertyValue)
                            return false;
                        break;
                    case ShippingContraintOperator.GreaterThan:
                        if (contraint.Value >= propertyValue)
                            return false;
                        break;
                    case ShippingContraintOperator.GreaterThanOrEqual:
                        if (contraint.Value > propertyValue)
                            return false;
                        break;
                    case ShippingContraintOperator.NotEqual:
                        if (contraint.Value == propertyValue)
                            return false;
                        break;
                }
            }

            return true;
        }

        private double EvalProperty(ShippingContraintProperty property, IList<Tuple<int, IShippingInfo>> ShippingInfos, Decimal ItemsTotal) {
            // Ensure ShippingInfos not null
            var shippingInfos = ShippingInfos ?? new List<Tuple<int, IShippingInfo>>();

            //  shippingInfos :
            //  Item1 => Qty
            //  Item2 => IShippingInfo
            switch (property) {
                case ShippingContraintProperty.TotalPrice:
                    return Convert.ToDouble(ItemsTotal);
                case ShippingContraintProperty.TotalWeight:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Sum(i => i.Item1 * i.Item2.Weight);
                case ShippingContraintProperty.TotalVolume:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Sum(i => i.Item1 * i.Item2.Length * i.Item2.Width * i.Item2.Height);
                case ShippingContraintProperty.ItemLongestDimension:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Max(i => new double[] { i.Item2.Length, i.Item2.Width, i.Item2.Height }.Max());
                case ShippingContraintProperty.MaxItemLength:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Max(i => i.Item2.Length);
                case ShippingContraintProperty.MaxItemWidth:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Max(i => i.Item2.Width);
                case ShippingContraintProperty.MaxItemHeight:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Max(i => i.Item2.Height);
                default:
                    return 0;
            }
        }
    }
}