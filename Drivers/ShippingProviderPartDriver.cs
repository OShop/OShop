using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using Orchard.Mvc;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingProviderPartDriver : ContentPartDriver<ShippingProviderPart> {
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IFeatureManager _featureManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IShippingService _shippingService;

        private const string TemplateName = "Parts/ShippingProvider";

        public ShippingProviderPartDriver(
            ICurrencyProvider currencyProvider,
            IFeatureManager featureManager,
            IHttpContextAccessor httpContextAccessor,
            IShippingService shippingService) {
            _currencyProvider = currencyProvider;
            _featureManager = featureManager;
            _httpContextAccessor = httpContextAccessor;
            _shippingService = shippingService;
        }

        protected override string Prefix { get { return "ShippingProvider"; } }

        // GET
        protected override DriverResult Editor(ShippingProviderPart part, dynamic shapeHelper) {
            var model = new ShippingProviderPartEditViewModel() {
                NumberFormat = _currencyProvider.NumberFormat,
                Part = part,
                VatEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.VAT").Any()
            };

            return ContentShape("Parts_ShippingProvider_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: model,
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(ShippingProviderPart part, IUpdateModel updater, dynamic shapeHelper) {
            var httpContext = _httpContextAccessor.Current();

            if (httpContext.Request.Form["submit.Save"] == "ShippingProvider.New"
                && httpContext.Request.Form["ShippingProvider.New.Option"] != null
                && !String.IsNullOrWhiteSpace(httpContext.Request.Form["ShippingProvider.New.Option"])) {
                // New option
                    _shippingService.CreateOption(new ShippingOptionRecord() {
                        Name = httpContext.Request.Form["ShippingProvider.New.Option"].Trim(),
                        Enabled = false,
                        ShippingProviderPartRecord = part.Record,
                        Priority = 0,
                        Price = 0
                    });

            }

            return Editor(part, shapeHelper);
        }
    }
}