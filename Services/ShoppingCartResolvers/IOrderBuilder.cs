using Orchard;
using Orchard.ContentManagement;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Services.ShoppingCartResolvers {
    public interface IOrderBuilder : IDependency {
        Int32 Priority { get; }
        void BuildOrder(IShoppingCartService ShoppingCartService, ref IContent Order);
    }
}
