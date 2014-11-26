using Newtonsoft.Json;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderPart : ContentPart<OrderPartRecord> {
        public string Reference {
            get { return this.Retrieve(x => x.Reference); }
            set { this.Store(x => x.Reference, value); }
        }

        public CustomerInfos CustomerInfos {
            get { return JsonConvert.DeserializeObject<CustomerInfos>(this.Retrieve(x => x.CustomerInfos)); }
            set { this.Store(x => x.CustomerInfos, JsonConvert.SerializeObject(value)); }
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
            get { return JsonConvert.DeserializeObject<IList<OrderItem>>(this.Retrieve(x => x.Items)); }
            set { this.Store(x => x.Items, JsonConvert.SerializeObject(value)); }
        }

        public IList<OrderLog> Logs {
            get { return JsonConvert.DeserializeObject<IList<OrderLog>>(this.Retrieve(x => x.Logs)); }
            set { this.Store(x => x.Logs, JsonConvert.SerializeObject(value)); }
        }
    }

    public struct CustomerInfos {
        public string FirstName;
        public string LastName;
        public string EMail;
        public string Phone;
    }

    public enum OrderStatus : int {
        Canceled = -1,
        Pending = 0,
        Processing = 1,
        Completed = 2
    }

    public struct OrderItem {
        public int Id;
        public string SKU;
        public int ContentId;
        public string Designation;
        public string Description;
        public decimal UnitPrice;
        public int Quantity;
        public int VatId;
    }

    public struct OrderLog {
        public DateTime LogDate;
        public String Description;
    }
}