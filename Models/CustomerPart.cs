using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Utilities;
using Orchard.Core.Common.Models;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OShop.Models {
    public class CustomerPart : ContentPart<CustomerPartRecord>, ITitleAspect {
        [Required]
        public String FirstName {
            get { return this.Retrieve(x => x.FirstName); }
            set { this.Store(x => x.FirstName, value); }
        }

        [Required]
        public String LastName {
            get { return this.Retrieve(x => x.LastName); }
            set { this.Store(x => x.LastName, value); }
        }

        public Int32 DefaultAddressId {
            get { return this.Retrieve(x => x.DefaultAddressId); }
            set { this.Store(x => x.DefaultAddressId, value); }
        }

        public String Title {
            get { return this.FirstName + " " + this.LastName; }
        }

        public String Email {
            get { return this.Owner.Email; }
        }

        public IUser Owner {
            get { return this.As<CommonPart>().Owner; }
            set { this.As<CommonPart>().Owner = value; }
        }
    }
}