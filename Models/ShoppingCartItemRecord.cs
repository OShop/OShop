using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShoppingCartItemRecord {
        public virtual int Id { get; set; }
        public virtual ShoppingCartRecord ShoppingCartRecord { get; set; }
        public virtual string ItemType { get; set; }
        public virtual int ItemId { get; set; }
        public virtual int Quantity { get; set; }
    }
}