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
    public class VatShippingProviderPartDriver : ContentPartDriver<ShippingProviderPart> {
        private readonly IVatService _vatService;
        private readonly ICurrencyProvider _currencyProvider;

        private const string TemplateName = "Parts/Vat";

        public VatShippingProviderPartDriver(
            IVatService vatService,
            ICurrencyProvider currencyProvider
            ) {
            _vatService = vatService;
            _currencyProvider = currencyProvider;
        }

        protected override string Prefix { get { return "ShippingProvider"; } }

        // GET
        protected override DriverResult Editor(ShippingProviderPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Vat_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: new VatEditViewModel {
                        VatRates = _vatService.ListVats().OrderBy(v => v.Name),
                        SelectedVatId = (part.VAT != null ? part.VAT.Id : 0)
                    },
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(ShippingProviderPart part, IUpdateModel updater, dynamic shapeHelper) {
            var model = new VatEditViewModel();
            if (updater.TryUpdateModel(model, Prefix, null, null)) {
                if (model.SelectedVatId > 0) {
                    part.VAT = _vatService.GetVat(model.SelectedVatId);
                }
                else {
                    part.VAT = null;
                }
            }
            return Editor(part, shapeHelper);
        }

    }
}