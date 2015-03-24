using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Models {
    public interface IShopItem : IContent, IPrice {
        string ItemType { get; }
        string SKU { get; }
        string Designation { get; }
        string Description { get; }
        decimal GetUnitPrice(int Quantity = 1);
    }
}
