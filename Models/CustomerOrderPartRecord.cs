using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class CustomerOrderPartRecord : ContentPartRecord {
        public virtual Int32 CustomerId { get; set; }
    }
}