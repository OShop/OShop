using Newtonsoft.Json;
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

        internal IDictionary<string, string> Properties {
            get {
                return this.Data != null ? JsonConvert.DeserializeObject<IDictionary<string, string>>(this.Data) : new Dictionary<string, string>();
            }
            set {
                this.Data = JsonConvert.SerializeObject(value);
            }
        }
    }
}