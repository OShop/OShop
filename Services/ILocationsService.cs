using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OShop.Services {
    public interface ILocationsService : IDependency {
        // Countries
        void AddCountry(LocationsCountryRecord record);
        void UpdateCountry(LocationsCountryRecord record);
        void DeleteCountry(int Id);
        void DeleteCountry(LocationsCountryRecord record);
        LocationsCountryRecord GetCountry(int Id);
        IEnumerable<LocationsCountryRecord> GetCountries();
        IEnumerable<LocationsCountryRecord> GetEnabledCountries();

        int GetDefaultCountryId();
        LocationsCountryRecord GetDefaultCountry();
        void SetDefaulCountryId(int Id);
        void SetDefaulCountry(LocationsCountryRecord record);

        // States
        void AddState(LocationsStateRecord record);
        void UpdateState(LocationsStateRecord record);
        void DeleteState(int Id);
        void DeleteState(LocationsStateRecord record);
        LocationsStateRecord GetState(int Id);
        IEnumerable<LocationsStateRecord> GetStates();
        IEnumerable<LocationsStateRecord> GetStates(int CountryId);
        IEnumerable<LocationsStateRecord> GetEnabledStates(int CountryId);

        // Addresses
        string FormatAddress(IOrderAddress address);

        // Import
        void Import(XDocument ImportedLocations);
    }
}
