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
    [OrchardFeature("OShop.Products")]
    public class OrderProductsPartDriver : ContentPartDriver<OrderProductsPart> {
        private readonly ICurrencyProvider _currencyProvider;

        public OrderProductsPartDriver(
            ICurrencyProvider currencyProvider
            ) {
            _currencyProvider = currencyProvider;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Display(OrderProductsPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Order_Products", () => shapeHelper.Parts_Order_Products(
                    ContentPart: part,
                    NumberFormat: _currencyProvider.NumberFormat)),
                ContentShape("Parts_Order_Products_SubTotal", () => shapeHelper.Parts_Order_SubTotal(
                    Label: T("Products total"),
                    SubTotal: part.ProductDetails.Sum(d => d.Total),
                    NumberFormat: _currencyProvider.NumberFormat))
            );
        }
    }
}