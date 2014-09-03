using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Models {
    public interface ICurrencySettings {
        string CurrencyIsoCode { get; }
        string CurrencySymbol { get; }
        int CurrencyDecimalDigits { get; }
        string CurrencyDecimalSeparator { get; }
        string CurrencyGroupSeparator { get; }
        int[] CurrencyGroupSizes { get; }
        int CurrencyNegativePattern { get; }
        int CurrencyPositivePattern { get; }
    }
}
