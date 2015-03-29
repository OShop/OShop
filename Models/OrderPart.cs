using Newtonsoft.Json;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Utilities;
using System;
using System.Collections.Generic;

namespace OShop.Models {
    public class OrderPart : ContentPart<OrderPartRecord>, ITitleAspect {
        internal readonly LazyField<List<OrderDetail>> _details = new LazyField<List<OrderDetail>>();

        public string Title {
            get { return this.Reference; }
        }

        public string Reference {
            get { return this.Retrieve(x => x.Reference); }
            set { this.Store(x => x.Reference, value); }
        }

        public OrderStatus OrderStatus {
            get { return (OrderStatus)this.Retrieve(x => x.OrderStatus); }
            set { this.Store(x => x.OrderStatus, value); }
        }

        public List<OrderDetail> Details {
            get { return _details.Value; }
        }

        public IList<OrderLog> Logs {
            get { return JsonConvert.DeserializeObject<IList<OrderLog>>(this.Retrieve(x => x.Logs, "")) ?? new List<OrderLog>(); }
            set { this.Store(x => x.Logs, JsonConvert.SerializeObject(value)); }
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