using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShippingContraint {
        public ShippingContraintProperty Property { get; set; }
        public ShippingContraintOperator Operator { get; set; }
        public float Value { get; set; }
    }

    public enum ShippingContraintProperty {
        TotalPrice,
        TotalWeight,
        TotalVolume,
        ItemLongestDimension,
        ItemLength,
        ItemWidth,
        ItemHeight
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