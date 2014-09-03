using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public struct NumberFormat {
        public string CurrencyDecimalSeparator;
        public string CurrencyGroupSeparator;
        public int[] CurrencyGroupSizes;
    }

    public static class NumberFormats {
        public static NumberFormat[] Formats = {
            // 1,234.56
            new NumberFormat {
                CurrencyDecimalSeparator = ".",
                CurrencyGroupSeparator = ",",
                CurrencyGroupSizes = new int[] {3}
            },
            // 1 234,56
            new NumberFormat {
                CurrencyDecimalSeparator = ",",
                CurrencyGroupSeparator = " ",
                CurrencyGroupSizes = new int[] {3}
            },
            // 1.234,56
            new NumberFormat {
                CurrencyDecimalSeparator = ",",
                CurrencyGroupSeparator = ".",
                CurrencyGroupSizes = new int[] {3}
            },
            // 1 234.56
            new NumberFormat {
                CurrencyDecimalSeparator = ".",
                CurrencyGroupSeparator = " ",
                CurrencyGroupSizes = new int[] {3}
            },
            // 1'234.56
            new NumberFormat {
                CurrencyDecimalSeparator = ".",
                CurrencyGroupSeparator = "'",
                CurrencyGroupSizes = new int[] {3}
            }
        };
    }
}