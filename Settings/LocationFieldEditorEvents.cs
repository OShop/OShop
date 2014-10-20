using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Settings {
    [OrchardFeature("OShop.Locations")]
    public class LocationFieldEditorEvents : ContentDefinitionEditorEventsBase {
        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition) {
            if (definition.FieldDefinition.Name == "LocationField") {
                var model = definition.Settings.GetModel<LocationFieldSettings>();
                yield return DefinitionTemplate(model);
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder, IUpdateModel updateModel) {
            if (builder.FieldType != "LocationField") {
                yield break;
            }

            var model = new LocationFieldSettings();
            if (updateModel.TryUpdateModel(model, "LocationFieldSettings", null, null)) {
                builder.WithSetting("LocationFieldSettings.Required", model.Required.ToString());
            }

            yield return DefinitionTemplate(model);
        }
    }
}