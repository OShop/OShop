using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Models {
    public interface IShippingInfo {
        bool RequiresShipping { get; }
        double Weight { get; }
        double Width { get; }
        double Height { get; }
        double Lenght { get; }
    }
}
