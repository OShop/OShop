using Orchard;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Services {
    public interface ICurrencyProvider : IDependency {
        String IsoCode { get; }
        String Symbol { get; }
        NumberFormatInfo NumberFormat { get; }
    }
}
