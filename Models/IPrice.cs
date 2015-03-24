using Orchard.ContentManagement;

namespace OShop.Models {
    public interface IPrice : IContent {
        decimal Price { get; }
    }
}
