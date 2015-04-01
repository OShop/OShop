using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace OShop.Migrations {
    [OrchardFeature("OShop.VAT")]
    public class VatMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("VatRatePartRecord", table => table
                 .ContentPartVersionRecord()
                 .Column<string>("Name")
                 .Column<decimal>("Rate"));

            SchemaBuilder.CreateTable("VatPartRecord", table => table
                 .ContentPartVersionRecord()
                 .Column<int>("VatRateId"));

            ContentDefinitionManager.AlterPartDefinition("VatRatePart", part => part
                .Attachable(false)
                );

            ContentDefinitionManager.AlterPartDefinition("VatPart", part => part
                .Attachable()
                .WithDescription("Adds VAT rate to you products.")
                );

            ContentDefinitionManager.AlterTypeDefinition("VatRate", type => type
                .WithPart("CommonPart")
                .WithPart("VatRatePart")
                .Creatable(false)
                );

            ContentDefinitionManager.AlterTypeDefinition("ShippingProvider", type => type
                .WithPart("VatPart")
                );

            ContentDefinitionManager.AlterTypeDefinition("Order", type => type
                .WithPart("OrderVatPart")
            );

            return 1;
        }

    }
}