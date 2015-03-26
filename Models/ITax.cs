using Orchard.ContentManagement;
using System;

namespace OShop.Models {
    public interface ITax {
        String Name { get; }
        Decimal Rate { get; }
    }
}
