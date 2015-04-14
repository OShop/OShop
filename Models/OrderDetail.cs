using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class OrderDetail {
        private OrderDetailRecord _record;

        public OrderDetail(String DetailType) {
            _record = new OrderDetailRecord() {
                DetailType = DetailType
            };
        }

        public OrderDetail(IShopItem Item, int Quantity) {
            _record = new OrderDetailRecord() {
                DetailType = Item.ItemType,
                ContentId = Item.ContentItem != null ? Item.ContentItem.Id : 0,
                SKU = Item.SKU,
                Designation = Item.Designation,
                Description = Item.Description,
                UnitPrice = Item.GetUnitPrice(Quantity),
                Quantity = Quantity
            };
        }

        public OrderDetail(OrderDetailRecord Record) {
            _record = Record;
        }

        internal OrderDetailRecord Record {
            get { return _record; }
        }

        public int Id {
            get { return _record.Id; }
        }

        public string DetailType {
            get { return _record.DetailType; }
            set { _record.DetailType = value; }
        }

        public int ContentId {
            get { return _record.ContentId; }
            set { _record.ContentId = value; }
        }

        public string SKU {
            get { return _record.SKU; }
            set { _record.SKU = value; }
        }

        public string Designation {
            get { return _record.Designation; }
            set { _record.Designation = value; }
        }

        public string Description {
            get { return _record.Description; }
            set { _record.Description = value; }
        }

        public decimal UnitPrice {
            get { return _record.UnitPrice; }
            set { _record.UnitPrice = value; }
        }

        public int Quantity {
            get { return _record.Quantity; }
            set { _record.Quantity = value; }
        }

        public decimal ReductionPercent {
            get { return _record.ReductionPercent; }
            set { _record.ReductionPercent = value; }
        }

        public decimal ReductionAmount {
            get { return _record.ReductionAmount; }
            set { _record.ReductionAmount = value; }
        }

        public decimal Total {
            get { return (UnitPrice - UnitPrice * ReductionPercent) * Quantity - ReductionAmount; }
        }

        #region Properties
        private JObject Properties {
            get {
                return _record.Data != null ? JObject.Parse(_record.Data) : new JObject();
            }
            set {
                _record.Data = value.ToString(Formatting.None);
            }
        }

        public void SetProperty<T>(string Key, T Value) {
            var _attributes = Properties;
            _attributes[Key] = JToken.FromObject(Value);
            Properties = _attributes;
        }

        public T GetProperty<T>(string Key) {
            return Properties[Key] != null ? Properties[Key].ToObject<T>() : default(T);
        }

        public void RemoveProperty(string Key) {
            var _attributes = Properties;
            _attributes.Remove(Key);
            Properties = _attributes;
        }
        
        #endregion
    }
}