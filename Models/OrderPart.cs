using Newtonsoft.Json;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using System;
using System.Collections.Generic;

namespace OShop.Models {
    public class OrderPart : ContentPart<OrderPartRecord>, ITitleAspect {
        public string Title {
            get { return this.Reference; }
        }

        public string Reference {
            get { return this.Retrieve(x => x.Reference); }
            set { this.Store(x => x.Reference, value); }
        }

        public string BillingAddress {
            get { return this.Retrieve(x => x.BillingAddress); }
            set { this.Store(x => x.BillingAddress, value); }
        }

        public OrderStatus OrderStatus {
            get { return (OrderStatus)this.Retrieve(x => x.OrderStatus); }
            set { this.Store(x => x.OrderStatus, value); }
        }

        public IList<OrderItem> Items {
            get { return JsonConvert.DeserializeObject<IList<OrderItem>>(this.Retrieve(x => x.Items, "")) ?? new List<OrderItem>(); }
            set { this.Store(x => x.Items, JsonConvert.SerializeObject(value)); }
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

    public class OrderItem {
        public int Id;
        public string SKU;
        public int ContentId;
        public string Designation;
        public string Description;
        public decimal UnitPrice;
        public int Quantity;
        public int VatId;
    }

    public class OrderLog {
        public DateTime LogDate;
        public String Description;
    }
}