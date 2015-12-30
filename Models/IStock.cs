using Orchard.ContentManagement;

namespace OShop.Models {
    public interface IStock : IContent {
        int? AvailableQty { get; }

        int? MaxOrderQty { get; }
    }
}
