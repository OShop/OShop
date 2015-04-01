using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.VAT")]
    public class OrderVatPartDriver : ContentPartDriver<OrderVatPart> {
        private readonly ICurrencyProvider _currencyProvider;

        public OrderVatPartDriver(ICurrencyProvider currencyProvider) {
            _currencyProvider = currencyProvider;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Display(OrderVatPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Order_Vat_SubTotal", () => shapeHelper.Parts_Order_SubTotal(
                    Label: T("VAT"),
                    SubTotal: part.SubTotal,
                    NumberFormat: _currencyProvider.NumberFormat))
            );
        }
    }
}