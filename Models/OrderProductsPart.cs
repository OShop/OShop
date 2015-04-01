using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderProductsPart : ContentPart, IOrderSubTotal {
        internal readonly LazyField<IEnumerable<OrderDetail>> _productDetails = new LazyField<IEnumerable<OrderDetail>>();

        public IEnumerable<OrderDetail> ProductDetails {
            get { return _productDetails.Value; }
        }

        public decimal SubTotal {
            get { return ProductDetails.Sum(d => d.Total); }
        }
    }
}