using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Helpers {
    public static class ShippingExtensions {
        public static bool IsShippingRequired(this IEnumerable<ItemShippingInfo> Items) {
            return Items.Where(i => i.ShippingInfo.RequiresShipping).Any();
        }
    }
}