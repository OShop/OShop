using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Models {
    public interface IShippingInfo {
        double Weight { get; }
        double Length { get; }
        double Width { get; }
        double Height { get; }
    }
}
