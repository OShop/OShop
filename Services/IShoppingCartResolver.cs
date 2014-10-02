using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Services {
    public interface IShoppingCartResolver : IDependency {
        Int32 Priority { get; }
        void ResolveCart(ref ShoppingCart Cart);
    }
}
