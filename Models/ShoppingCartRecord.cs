using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShoppingCartRecord {
        public virtual int Id { get; set; }
        public virtual Guid Guid { get; set; }
        public virtual DateTime ModifiedUtc { get; set; }
        public virtual int? OwnerId { get; set; }
        public virtual IList<ShoppingCartItemRecord> Items { get; set; }

        internal ShoppingCartRecord() {
            Items = new List<ShoppingCartItemRecord>();
        }
    }
}