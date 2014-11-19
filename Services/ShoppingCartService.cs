using Newtonsoft.Json.Linq;
using Orchard;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Services;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Services {
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartService : IShoppingCartService {
        private const string ShoppingCartKey = "ShoppingCartId";

        private readonly IRepository<ShoppingCartRecord> _shoppingCartRepository;
        private readonly IRepository<ShoppingCartItemRecord> _shoppingCartItemRepository;
        private readonly IEnumerable<IShoppingCartResolver> _shoppingCartResolvers;
        private readonly IClock _clock;

        public ShoppingCartService(
            IRepository<ShoppingCartRecord> shoppingCartRepository,
            IRepository<ShoppingCartItemRecord> shoppingCartItemRepository,
            IEnumerable<IShoppingCartResolver> shoppingCartResolvers,
            IClock clock,
            IOrchardServices services) {
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _shoppingCartResolvers = shoppingCartResolvers;
            _clock = clock;
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        private ShoppingCartRecord GetCartRecord(bool CreateIfNull = false) {
            Guid shoppingCartGuid = Guid.Empty;

            if (Services.WorkContext.HttpContext.Session[ShoppingCartKey] != null) {
                shoppingCartGuid = (Guid)Services.WorkContext.HttpContext.Session[ShoppingCartKey];
            }

            ShoppingCartRecord cart = null;

            if (shoppingCartGuid != null && shoppingCartGuid != Guid.Empty) {
                // try return session cart
                cart = GetCartRecord(shoppingCartGuid);
            }

            if (cart == null && Services.WorkContext.CurrentUser != null) {
                // try return user cart
                cart = GetUserCartRecord(Services.WorkContext.CurrentUser.Id);
            }
            else if (cart != null && !cart.OwnerId.HasValue && Services.WorkContext.CurrentUser != null) {
                // Attach cart to user
                cart.OwnerId = Services.WorkContext.CurrentUser.Id;
            }

            if (cart == null && CreateIfNull) {
                return CreateCartRecord();
            }
            else {
                return cart;
            }
        }

        private ShoppingCartRecord GetCartRecord(Guid Guid) {
            return _shoppingCartRepository.Get(sc => sc.Guid == Guid);
        }

        private ShoppingCartRecord GetUserCartRecord(int OwnerId) {
            var userCarts = _shoppingCartRepository
                .Fetch(sc => sc.OwnerId.HasValue && sc.OwnerId == OwnerId)
                .OrderByDescending(sc => sc.ModifiedUtc)
                .ToList();

            if (userCarts.Count > 1) {
                // Remove old carts
                foreach (var cart in userCarts.Skip(1)) {
                    _shoppingCartRepository.Delete(cart);
                }
            }

            return userCarts.FirstOrDefault();
        }

        private ShoppingCartRecord CreateCartRecord() {
            var cart = new ShoppingCartRecord() {
                Guid = Guid.NewGuid(),
                ModifiedUtc = _clock.UtcNow
            };

            if (Services.WorkContext.CurrentUser != null) {
                // Attach cart to user
                cart.OwnerId = Services.WorkContext.CurrentUser.Id;
            }

            // Register cart
            Services.WorkContext.HttpContext.Session[ShoppingCartKey] = cart.Guid;

            _shoppingCartRepository.Create(cart);

            return cart;
        }

        #region IShoppingCartService members

        public IEnumerable<ShoppingCartItemRecord> ListItems() {
            var cart = GetCartRecord();
            if (cart != null) {
                return cart.Items;
            }
            else {
                return new List<ShoppingCartItemRecord>();
            }
        }

        public void Add(int ItemId, string ItemType = ProductPart.PartItemType, int Quantity = 1) {
            var cart = GetCartRecord(CreateIfNull: true);

            var item = cart.Items.Where(i=>i.ItemId == ItemId && i.ItemType == ItemType).FirstOrDefault();
            if (item != null) {
                // Existing cart item
                item.Quantity += Quantity;
            }
            else {
                // New cart item
                _shoppingCartItemRepository.Create(new ShoppingCartItemRecord() {
                    ShoppingCartRecord = cart,
                    ItemType = ItemType,
                    ItemId = ItemId,
                    Quantity = Quantity
                });
            }

            cart.ModifiedUtc = _clock.UtcNow;
        }

        public void UpdateQuantity(int Id, int Quantity) {
            if (Quantity <= 0) {
                Remove(Id);
            }

            var item = _shoppingCartItemRepository.Get(Id);

            if (item != null) {
                if (item.Quantity != Quantity) {
                    item.Quantity = Quantity;
                }

                item.ShoppingCartRecord.ModifiedUtc = _clock.UtcNow;
            }
        }

        public void Remove(int Id) {
            var item = _shoppingCartItemRepository.Get(Id);

            if (item != null) {
                if (item.ShoppingCartRecord.Items.Where(r => r.Id != Id).Any()) {
                    // cart still contains items
                    item.ShoppingCartRecord.ModifiedUtc = _clock.UtcNow;

                    item.ShoppingCartRecord.Items.Remove(item);
                    //_shoppingCartItemRepository.Delete(item);
                }
                else {
                    // cart will be empty
                    Empty();
                }
            }
        }

        public void Empty() {
            var cart = GetCartRecord();

            if (cart != null) {
                _shoppingCartRepository.Delete(cart);
            }

            // Unregister cart
            Services.WorkContext.HttpContext.Session.Remove(ShoppingCartKey);
        }

        public void SetProperty<T>(string Key, T Value) {
            var cart = GetCartRecord();

            if (cart != null) {
                var properties = cart.Properties;
                properties[Key] = JToken.FromObject(Value);
                cart.Properties = properties;
            }
        }

        public T GetProperty<T>(string Key) {
            var cart = GetCartRecord();

            if (cart != null) {
                return cart.Properties.Value<T>(Key);
            }

            return default(T);
        }

        public void RemoveProperty(string Key) {
            var cart = GetCartRecord();

            if (cart != null) {
                var properties = cart.Properties;
                properties.Remove(Key);
                cart.Properties = properties;
            }
        }

        #endregion

        public ShoppingCart GetShoppingCart() {
            ShoppingCart cart = new ShoppingCart();
            foreach (var resolver in _shoppingCartResolvers.OrderByDescending(r => r.Priority)) {
                resolver.ResolveCart(this, ref cart);
            }
            return cart;
        }

    }
}