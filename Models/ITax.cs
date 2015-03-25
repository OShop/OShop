using Orchard.ContentManagement;
using System;

namespace OShop.Models {
    public interface ITax : IContent {
        String Name { get; }
        Decimal Rate { get; }
    }
}
