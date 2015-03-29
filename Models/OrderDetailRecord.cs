using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderDetailRecord {
        public virtual int Id { get; set; }
        public virtual int OrderId { get; set; }
        public virtual int ContentId { get; set; }
        public virtual int ContentVersionId { get; set; }
        public virtual int Quantity { get; set; }
    }
}