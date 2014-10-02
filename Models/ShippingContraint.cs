using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShippingContraint {
        public ShippingContraintProperty Property { get; set; }
        public ShippingContraintOperator Operator { get; set; }
        public double Value { get; set; }
    }

    public enum ShippingContraintProperty {
        TotalPrice,
        TotalWeight,
        TotalVolume,
        ItemLongestDimension,
        MaxItemLength,
        MaxItemWidth,
        MaxItemHeight
    }

    public enum ShippingContraintOperator {
        LessThan,
        LessThanOrEqual,
        Equal,
        GreaterThan,
        GreaterThanOrEqual,
        NotEqual
    }
}