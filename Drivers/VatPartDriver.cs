using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Notify;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Drivers {
    [OrchardFeature("OShop.VAT")]
    public class VatPartDriver : ContentPartDriver<VatPart> {
        private readonly IVatService _vatService;

        private const string TemplateName = "Parts/Vat";

        public VatPartDriver(
            IVatService vatService,
            IOrchardServices services
            ) {
            _vatService = vatService;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        protected override string Prefix { get { return "Vat"; } }

        protected override DriverResult Display(VatPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Vat", () => shapeHelper.Parts_Vat(
                    ContentPart: part
                )),
                ContentShape("Parts_Vat_SummaryAdmin", () => shapeHelper.Parts_Vat_SummaryAdmin(
                    ContentPart: part
                ))
            );
        }

        // GET
        protected override DriverResult Editor(VatPart part, dynamic shapeHelper) {
            var vatRates = _vatService.ListVatRates();
            if (!vatRates.Any()) {
                Services.Notifier.Warning(T("There is no VAT rate available, please create one."));
            }

            return ContentShape("Parts_Vat_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: new VatEditViewModel() {
                        VatRates = vatRates,
                        SelectedRateId = part.VatRate != null ? part.VatRate.Id : 0
                    },
                    Prefix: Prefix
                )
            );
        }

        // POST
        protected override DriverResult Editor(VatPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper) {
            var model = new VatEditViewModel();
            if (updater.TryUpdateModel(model, Prefix, null, null)) {
                part.VatRate = _vatService.GetVatRate(model.SelectedRateId);
            }

            return Editor(part, shapeHelper);
        }
    }
}