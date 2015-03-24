using Orchard;
using Orchard.ContentManagement;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OShop.Services.ShoppingCartResolvers {
    public interface IShoppingCartBuilder : IDependency {
        Int32 Priority { get; }
        void BuildCart(IShoppingCartService ShoppingCartService, ShoppingCart Cart);
    }
}
