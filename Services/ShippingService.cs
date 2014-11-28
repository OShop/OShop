using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

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

        public ShippingOptionRecord GetOption(int Id) {
            return _optionRepository.Get(Id);
        }

        public IEnumerable<ShippingOptionRecord> GetOptions(ShippingProviderPart part) {
            return _optionRepository.Fetch(o => o.ShippingProviderId == part.Id);
        }

        public IEnumerable<ShippingProviderOption> GetSuitableProviderOptions(ShoppingCart cart) {
            var publishedProviders = _contentManager.Query<ShippingProviderPart>(VersionOptions.Published).List();

            List<ShippingProviderOption> suitableProviders = new List<ShippingProviderOption>();
            foreach (var provider in publishedProviders) {
                var option = GetSuitableOption(provider.Id, cart);
                if (option != null) {
                    suitableProviders.Add(new ShippingProviderOption(provider, option));
                }
            }

            return suitableProviders;
        }

        #endregion

        private ShippingOptionRecord GetSuitableOption(int ShippingProviderId, ShoppingCart cart) {
            var shippingZone = cart.Properties["ShippingZone"] as ShippingZoneRecord;
            if (shippingZone == null || !shippingZone.Enabled) {
                return null;
            }

            return _optionRepository
                .Fetch(o => o.ShippingProviderId == ShippingProviderId && o.ShippingZoneRecord == shippingZone && o.Enabled)
                .OrderByDescending(o => o.Priority)
                .Where(o => MeetsContraints(o, cart))
                .FirstOrDefault();
        }

        private bool MeetsContraints(ShippingOptionRecord option, ShoppingCart cart) {
            foreach (var contraint in option.Contraints) {
                double propertyValue = EvalProperty(contraint.Property, cart);
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

        private double EvalProperty(ShippingContraintProperty property, ShoppingCart cart) {
            var shippingInfos = cart.Items.Where(i => i.ShippingInfo != null && i.ShippingInfo.RequiresShipping);
            switch (property) {
                case ShippingContraintProperty.TotalPrice:
                    return Convert.ToDouble(cart.ItemsTotal());
                case ShippingContraintProperty.TotalWeight:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Sum(i => i.Quantity * i.ShippingInfo.Weight);
                case ShippingContraintProperty.TotalVolume:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Sum(i => i.Quantity * i.ShippingInfo.Length * i.ShippingInfo.Width * i.ShippingInfo.Height);
                case ShippingContraintProperty.ItemLongestDimension:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Max(i => new double[] { i.ShippingInfo.Length, i.ShippingInfo.Width, i.ShippingInfo.Height }.Max());
                case ShippingContraintProperty.MaxItemLength:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Max(i => i.ShippingInfo.Length);
                case ShippingContraintProperty.MaxItemWidth:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Max(i => i.ShippingInfo.Width);
                case ShippingContraintProperty.MaxItemHeight:
                    if (!shippingInfos.Any()) {
                        return 0;
                    }
                    return shippingInfos.Max(i => i.ShippingInfo.Height);
                default:
                    return 0;
            }
        }

    }
}