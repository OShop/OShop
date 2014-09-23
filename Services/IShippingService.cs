using Orchard;
using OShop.Models;
using System.Collections.Generic;

namespace OShop.Services {
    public interface IShippingService : IDependency {
        void CreateZone(ShippingZoneRecord record);
        void UpdateZone(ShippingZoneRecord record);
        void DeleteZone(int ZoneId);
        void DeleteZone(ShippingZoneRecord record);
        ShippingZoneRecord GetZone(int Id);
        IEnumerable<ShippingZoneRecord> GetZones();
        IEnumerable<ShippingZoneRecord> GetEnabledZones();
    }
}
