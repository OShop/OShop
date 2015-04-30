using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Models {
    public interface IPayable : IContent {
        string Reference { get; }
        decimal PayableAmount { get; }
    }
}
