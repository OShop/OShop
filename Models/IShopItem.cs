using Orchard.ContentManagement;

namespace OShop.Models {
    public interface IShopItem : IContent, IPrice {
        string ItemType { get; }
        string SKU { get; }
        string Designation { get; }
        string Description { get; }
        decimal GetUnitPrice(int Quantity = 1);
    }
}
