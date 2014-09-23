using Orchard;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Services;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartService : IShoppingCartService {
        private const string ShoppingCartKey = "ShoppingCartId";

        private readonly IRepository<ShoppingCartRecord> _shoppingCartRepository;
        private readonly IRepository<ShoppingCartItemRecord> _shoppingCartItemRepository;
        private readonly IEnumerable<IShopItemProvider> _shopItemProviders;
        private readonly IClock _clock;

        public ShoppingCartService(
            IRepository<ShoppingCartRecord> shoppingCartRepository,
            IRepository<ShoppingCartItemRecord> shoppingCartItemRepository,
            IEnumerable<IShopItemProvider> shopItemProviders,
            IClock clock,
            IOrchardServices services) {
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _shopItemProviders = shopItemProviders;
            _clock = clock;
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        private ShoppingCartRecord GetCart(bool CreateIfNull = false) {
            Guid shoppingCartGuid = Guid.Empty;

            if (Services.WorkContext.HttpContext.Session[ShoppingCartKey] != null) {
                shoppingCartGuid = (Guid)Services.WorkContext.HttpContext.Session[ShoppingCartKey];
            }

            ShoppingCartRecord cart = null;

            if (shoppingCartGuid != null && shoppingCartGuid != Guid.Empty) {
                // try return session cart
                cart = GetCart(shoppingCartGuid);
            }

            if (cart == null && Services.WorkContext.CurrentUser != null) {
                // try return user cart
                cart = GetUserCart(Services.WorkContext.CurrentUser.Id);
            }
            else if (cart != null && !cart.OwnerId.HasValue && Services.WorkContext.CurrentUser != null) {
                // Attach cart to user
                cart.OwnerId = Services.WorkContext.CurrentUser.Id;
            }

            if (cart == null && CreateIfNull) {
                return CreateCart();
            }
            else {
                return cart;
            }
        }

        private ShoppingCartRecord GetCart(Guid Guid) {
            return _shoppingCartRepository.Get(sc => sc.Guid == Guid);
        }

        private ShoppingCartRecord GetUserCart(int OwnerId) {
            return _shoppingCartRepository.Get(sc => sc.OwnerId.HasValue && sc.OwnerId == OwnerId);
        }

        private ShoppingCartRecord CreateCart() {
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

        public IEnumerable<ShoppingCartItem> ListItems() {
            var result = new List<ShoppingCartItem>();
            var cart = GetCart();
            if (cart != null) {
                var items = cart.Items;
                foreach (IShopItemProvider provider in _shopItemProviders.OrderByDescending(p=>p.Priority)) {
                    provider.GetItems(cart.Items, ref result);
                }
            }

            return result;
        }

        public void Add(int ItemId, string ItemType = ProductPart.PartItemType, int Quantity = 1) {
            var cart = GetCart(CreateIfNull: true);

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
            var cart = GetCart();

            if (cart != null) {
                _shoppingCartRepository.Delete(cart);
            }

            // Unregister cart
            Services.WorkContext.HttpContext.Session.Remove(ShoppingCartKey);
        }
        
        #endregion

    }
}