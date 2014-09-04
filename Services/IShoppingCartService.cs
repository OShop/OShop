using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Services {
    public interface IShoppingCartService : IDependency {
        IShoppingCart GetCart();
        IShoppingCart GetCart(int Id);
        IShoppingCart GetCart(Guid Guid);
    }
}
