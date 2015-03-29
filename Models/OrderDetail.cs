using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderDetail {
        public int Id;
        public IShopItem Item;
        public int Quantity;
    }
}