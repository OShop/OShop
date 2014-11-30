using Newtonsoft.Json;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderShippingPart : ContentPart<OrderShippingPartRecord> {
        public string ShippingAddress {
            get { return this.Retrieve(x => x.ShippingAddress); }
            set { this.Store(x => x.ShippingAddress, value); }
        }

        public ShippingStatus ShippingStatus {
            get { return (ShippingStatus)this.Retrieve(x => x.ShippingStatus); }
            set { this.Store(x => x.ShippingStatus, (int)value); }
        }

        public OrderShippingInfos ShippingInfos {
            get { return JsonConvert.DeserializeObject<OrderShippingInfos>(this.Retrieve(x => x.ShippingInfos, "")); }
            set { this.Store(x => x.ShippingInfos, JsonConvert.SerializeObject(value)); }
        }
    }

    public enum ShippingStatus : int {
        NotRequired = 0,
        Pending = 1,
        Shipped = 2,
        Delivered = 3
    }

    public class OrderShippingInfos {
        public string Designation;
        public string Description;
        public decimal Price;
        public int VatId;
    }
}