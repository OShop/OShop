using Newtonsoft.Json;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Models {
    public class OrderPart : ContentPart<OrderPartRecord>, ITitleAspect, IPayable {
        internal readonly LazyField<List<OrderDetail>> _details = new LazyField<List<OrderDetail>>();
        internal readonly LazyField<decimal> _orderTotal = new LazyField<decimal>();
        internal readonly LazyField<OrderAddressRecord> _billingAddress = new LazyField<OrderAddressRecord>();

        public string Title {
            get { return this.Reference; }
        }

        public string Reference {
            get { return this.Retrieve(x => x.Reference); }
            set { this.Store(x => x.Reference, value); }
        }

        public int BillingAddressId {
            get { return this.Retrieve(x => x.BillingAddressId); }
            set { this.Store(x => x.BillingAddressId, value); }
        }

        public OrderAddressRecord BillingAddress {
            get { return _billingAddress.Value; }
            set { _billingAddress.Value = value; }
        }

        public OrderStatus OrderStatus {
            get { return (OrderStatus)this.Retrieve(x => x.OrderStatus); }
            set { this.Store(x => x.OrderStatus, (int)value); }
        }

        public decimal OrderTotal {
            get { return _orderTotal.Value; }
            internal set { this.Store(x => x.OrderTotal, value); }
        }

        public List<OrderDetail> Details {
            get { return _details.Value; }
        }

        decimal IPayable.PayableAmount {
            get { return this.OrderTotal; }
        }
    }

    public enum OrderStatus : int {
        Canceled = -1,
        Pending = 0,
        Processing = 1,
        Completed = 2
    }

    public class OrderLog {
        public DateTime LogDate;
        public String Description;
    }
}