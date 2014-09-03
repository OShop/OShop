using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Products")]
    public class ProductPartDriver : ContentPartDriver<ProductPart> {
        private readonly ICurrencyProvider _currencyProvider;

        private const string TemplateName = "Parts/Product";

        public ProductPartDriver(ICurrencyProvider currencyProvider) {
            _currencyProvider = currencyProvider;
        }

        protected override string Prefix { get { return "Product"; } }

        protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Product_Price", () => shapeHelper.Parts_Product_Price(
                    NumberFormat: _currencyProvider.NumberFormat,
                    ContentPart: part,
                    UnitPrice: part.UnitPrice)),
                ContentShape("Parts_Product_Sku", () => shapeHelper.Parts_Product_Sku(
                    NumberFormat: _currencyProvider.NumberFormat,
                    ContentPart: part,
                    SKU: part.SKU)));
        }

        // GET
        protected override DriverResult Editor(ProductPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Product_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(ProductPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}