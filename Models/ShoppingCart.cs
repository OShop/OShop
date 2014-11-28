using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShoppingCart {
        private Boolean _isValid;
        public ShoppingCart() {
            _isValid = true;
            Items = new List<ShoppingCartItem>();
            Properties = new Hashtable();
        }

        /// <summary>
        /// Cart's content
        /// </summary>
        public List<ShoppingCartItem> Items;

        /// <summary>
        /// Cart's properties bag
        /// </summary>
        public Hashtable Properties;

        /// <summary>
        /// To know if cart can be checked out
        /// </summary>
        public Boolean IsValid {
            get {
                return Items.Any() && _isValid;
            }
        }

        /// <summary>
        /// Prevents cart to be checked out
        /// </summary>
        public void InvalidCart() {
            _isValid = false;
        }
    }
}