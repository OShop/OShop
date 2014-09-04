using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShoppingCartItem {
        public virtual int Id { get; set; }
        public virtual ShoppingCartRecord ShoppingCartRecord { get; set; }
        public virtual int ProductId { get; set; }
        public virtual string ItemType { get; set; }
        public virtual int ItemId { get; set; }
    }
}