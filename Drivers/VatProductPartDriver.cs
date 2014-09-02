using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.VAT")]
    public class VatProductPartDriver : ContentPartDriver<ProductPart> {
        private readonly IVatService _vatService;

        private const string TemplateName = "Parts/Product.Vat";

        public VatProductPartDriver(IVatService vatService) {
            _vatService = vatService;
        }

        protected override string Prefix { get { return "Product"; } }

        protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Product_Vat", () => shapeHelper.Parts_Product_Vat(
                    ContentPart: part,
                    Vat: part.VAT));
        }

        // GET
        protected override DriverResult Editor(ProductPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Product_Vat_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: BuildEditorViewModel(part),
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(ProductPart part, IUpdateModel updater, dynamic shapeHelper) {
            var model = new ProductVatEditViewModel();
            if (updater.TryUpdateModel(model, Prefix, null, null)) {
                int vatId = int.Parse(model.SelectedVatId);
                part.VAT = _vatService.GetVat(vatId);
            }
            return Editor(part, shapeHelper);
        }

        private ProductVatEditViewModel BuildEditorViewModel(ProductPart part) {
            var pvvm = new ProductVatEditViewModel {
                VatRates = _vatService.ListVats().OrderBy(v => v.Name),
                SelectedVatId = (part.VAT != null ? part.VAT.Id.ToString() : string.Empty)
            };
            return pvvm;
        }
    }
}