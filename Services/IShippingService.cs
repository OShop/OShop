using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;

namespace OShop.Services {
    public interface IShippingService : IDependency {
        #region Shipping zones
        void CreateZone(ShippingZoneRecord record);
        void UpdateZone(ShippingZoneRecord record);
        void DeleteZone(int ZoneId);
        void DeleteZone(ShippingZoneRecord record);
        ShippingZoneRecord GetZone(int Id);
        IEnumerable<ShippingZoneRecord> GetZones();
        IEnumerable<ShippingZoneRecord> GetEnabledZones();
        
        #endregion

        #region Shipping options
        void CreateOption(ShippingOptionRecord record);
        void UpdateOption(ShippingOptionRecord record);
        void DeleteOption(int OptionId);
        void DeleteOption(ShippingOptionRecord record);
        ShippingProviderPart GetProvider(Int32 ProviderId, Int32? VersionRecordId = null);
        ShippingOptionRecord GetOption(int Id);
        IEnumerable<ShippingOptionRecord> GetOptions(ShippingProviderPart part);
        IEnumerable<ShippingProviderOption> GetSuitableProviderOptions(ShippingZoneRecord Zone, IList<Tuple<int, IShippingInfo>> ShippingInfos, Decimal ItemsTotal = 0);
        #endregion
    }
}
