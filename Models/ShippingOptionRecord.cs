using System.Collections.Generic;
using Newtonsoft.Json;
using Orchard.Data.Conventions;

namespace OShop.Models {
    public class ShippingOptionRecord {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual ShippingZoneRecord ShippingZoneRecord { get; set; }
        public virtual int ShippingProviderId { get; set; }
        public virtual int Priority { get; set; }
        [StringLengthMax]
        public virtual string Data { get; set; }
        public virtual decimal Price { get; set; }

        internal IList<ShippingContraint> Contraints {
            get {
                return this.Data != null ? JsonConvert.DeserializeObject<IList<ShippingContraint>>(this.Data) : new List<ShippingContraint>();
            }
            set {
                this.Data = JsonConvert.SerializeObject(value);
            }
        }
    }
}