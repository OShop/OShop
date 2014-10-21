using OShop.Models;
using OShop.Settings;
using System.Collections.Generic;

namespace OShop.ViewModels {
    public class LocationFieldViewModel {
        public LocationFieldSettings Settings { get; set; }
        public IEnumerable<LocationsCountryRecord> Countries { get; set; }
        public IEnumerable<LocationsStateRecord> States { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
    }
}