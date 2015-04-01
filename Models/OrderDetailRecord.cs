using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderDetailRecord {
        public virtual int Id { get; set; }
        public virtual int OrderId { get; set; }
        public virtual string DetailType { get; set; }
        public virtual int ContentId { get; set; }
        public virtual string SKU { get; set; }
        public virtual string Designation { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal ReductionPercent { get; set; }
        public virtual decimal ReductionAmount { get; set; }
        public virtual string Data { get; set; }
    }
}