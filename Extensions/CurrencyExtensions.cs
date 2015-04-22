using Orchard;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OShop.Extensions {
    public static class CurrencyExtensions {
        public static NumberFormatInfo NumberFormat(this WorkContext wContext) {
            return wContext.GetState<NumberFormatInfo>(OShop.Services.CurrencyWorkContext.CurrencyNumberFormatName);
        }
    }
}