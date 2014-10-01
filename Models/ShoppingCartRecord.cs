using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orchard.Data.Conventions;
using System;
using System.Collections.Generic;

namespace OShop.Models {
    public class ShoppingCartRecord {
        public virtual int Id { get; set; }
        public virtual Guid Guid { get; set; }
        public virtual DateTime ModifiedUtc { get; set; }
        public virtual int? OwnerId { get; set; }
        [StringLengthMax]
        public virtual string Data { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<ShoppingCartItemRecord> Items { get; set; }

        internal ShoppingCartRecord() {
            Items = new List<ShoppingCartItemRecord>();
        }

        internal JObject Properties {
            get {
                return this.Data != null ? JObject.Parse(this.Data) : new JObject();
            }
            set {
                this.Data = new JObject(value).ToString(Formatting.None);
            }
        }
    }
}