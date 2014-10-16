using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class CustomerPartRecord : ContentPartRecord {
        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual DateTime CreatedUtc { get; set; }
        public virtual Int32? UserId { get; set; }
    }
}