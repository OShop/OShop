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
        internal readonly LazyField<IEnumerable<CustomerAddressPart>> _addresses = new LazyField<IEnumerable<CustomerAddressPart>>();
        internal readonly LazyField<IUser> _user = new LazyField<IUser>();

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

        [EmailAddress]
        public String Email {
            get { return this.User != null ? this.User.Email : this.Retrieve(x => x.Email); }
            set { this.Store(x => x.Email, value); }
        }

        public Int32 UserId {
            get { return this.Retrieve(x => x.UserId); }
            set { this.Store(x => x.UserId, value); }
        }

        public IUser User {
            get { return _user.Value; }
            set { _user.Value = value; }
        }

        public IEnumerable<CustomerAddressPart> Addresses {
            get { return _addresses.Value; }
        }
    }
}