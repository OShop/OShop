using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Tokens;
using OShop.Models;

namespace OShop.Tokens {
    [OrchardFeature("OShop.Locations")]
    public class LocationsTokens : ITokenProvider {
        public LocationsTokens() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context) {
            context.For("OrderAddress", T("Order Address"), T("Tokens for order address"))
                .Token("Company", T("Company name"), T("Company name"))
                .Token("FirstName", T("First name"), T("First name"))
                .Token("LastName", T("Last name"), T("Last name"))
                .Token("Address1", T("Address line 1"), T("Address line 1"))
                .Token("Address2", T("Address line 2"), T("Address line 2"))
                .Token("Zipcode", T("Zip Code"), T("Zip Code"))
                .Token("City", T("City"), T("City"))
                .Token("Country", T("Country"), T("Country"), "Country")
                .Token("State", T("State"), T("State"), "State")
                ;

            context.For("Country", T("Country"), T("Tokens for country"))
                .Token("Name", T("Country name"), T("Country name"))
                .Token("IsoCode", T("Country ISO code"), T("Country ISO code"))
                ;

            context.For("State", T("State"), T("Tokens for State"))
                .Token("Name", T("State name"), T("State name"))
                .Token("IsoCode", T("State ISO code"), T("State ISO code"))
                ;
        }

        public void Evaluate(EvaluateContext context) {
            context.For<IOrderAddress>("OrderAddress")
                .Token("Company", address => address.Company)
                .Token("FirstName", address => address.FirstName)
                .Token("LastName", address => address.LastName)
                .Token("Address1", address => address.Address1)
                .Token("Address2", address => address.Address2)
                .Token("Zipcode", address => address.Zipcode)
                .Token("City", address => address.City)
                .Token("Country", address => address.Country.Name)
                .Chain("Country", "Country", address => address.Country)
                .Token("State", address => address.State.Name)
                .Chain("State", "State", address => address.State)
                ;
            context.For<LocationsCountryRecord>("Country")
                .Token("Name", country => country.Name)
                .Token("IsoCode", country => country.IsoCode)
                ;
            context.For<LocationsStateRecord>("State")
                .Token("Name", state => state.Name)
                .Token("IsoCode", state => state.IsoCode)
                ;
        }
    }
}