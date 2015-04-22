using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    public class CurrencyWorkContext : IWorkContextStateProvider {
        private readonly ICurrencyProvider _currencyProvider;

        public CurrencyWorkContext(ICurrencyProvider currencyProvider) {
            _currencyProvider = currencyProvider;
        }

        public Func<WorkContext, T> Get<T>(string name) {
            if (name == "CurrentNumberFormat") {
                return ctx => (T)(object)_currencyProvider.NumberFormat;
            }
            return null;
        }
    }
}