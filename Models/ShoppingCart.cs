using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShoppingCart {
        private Boolean _isValid;
        private List<TaxAmount> _taxes;

        public ShoppingCart() {
            _isValid = true;
            Items = new List<ShoppingCartItem>();
            _taxes = new List<TaxAmount>();
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
        /// Shipping option
        /// </summary>
        public Object Shipping;

        /// <summary>
        /// Tax summary
        /// </summary>
        public List<TaxAmount> Taxes {
            get { return _taxes; }
        }

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

        public void AddTax(ITax tax, decimal taxBase) {
            var taxAmount = _taxes.Where(ta => ta.Tax.Name == tax.Name && ta.Tax.Rate == tax.Rate).FirstOrDefault();
            if (taxAmount == null) {
                taxAmount = new TaxAmount(tax);
                _taxes.Add(taxAmount);
            }
            taxAmount.TaxBase += taxBase;
        }
    }

    public class TaxAmount {
        public TaxAmount(ITax tax) {
            Tax = tax;
        }

        public decimal TaxBase;
        public ITax Tax;
    }
}