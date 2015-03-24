using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Handlers {
    [OrchardFeature("OShop.VAT")]
    public class VatPartHandler : ContentHandler {
        private IVatService _vatService;

        public VatPartHandler(
            IVatService vatService,
            IRepository<VatPartRecord> repository
            ) {
            _vatService = vatService;

            Filters.Add(StorageFilter.For(repository));

            OnActivated<VatPart>((context, part) => {
                part._vatRate.Loader(rate => part.VatRateId > 0 ? _vatService.GetVatRate(part.VatRateId, part.VatRateVersionId) : null);
                part._vatRate.Setter(rate => {
                    if (rate != null && rate.ContentItem != null) {
                        part.VatRateId = rate.Id;
                        part.VatRateVersionId = rate.ContentItem.VersionRecord.Id;
                    }
                    else {
                        part.VatRateId = 0;
                        part.VatRateVersionId = 0;
                    }
                    return rate;
                });
            });
        }
    }
}