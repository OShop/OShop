using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace OShop.Migrations {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("ShippingPartRecord", table => table
                 .ContentPartVersionRecord()
                 .Column<bool>("RequiresShipping")
                 .Column<double>("Weight")
                 .Column<double>("Length")
                 .Column<double>("Width")
                 .Column<double>("Height"));

            SchemaBuilder.CreateTable("ShippingProviderPartRecord", table => table
                .ContentPartRecord()
                .Column<int>("VatRecord_Id"));

            SchemaBuilder.CreateTable("ShippingZoneRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<bool>("Enabled")
                 .Column<string>("Name"));

            SchemaBuilder.CreateTable("ShippingOptionRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<string>("Name")
                 .Column<bool>("Enabled")
                 .Column<int>("ShippingZoneRecord_Id")
                 .Column<int>("ShippingProviderId")
                 .Column<int>("Priority")
                 .Column<string>("Data", c => c.Unlimited())
                 .Column<decimal>("Price"));

            ContentDefinitionManager.AlterPartDefinition("ShippingPart", part => part
                 .Attachable()
                 .WithDescription("Add shipping information to your products"));

            ContentDefinitionManager.AlterPartDefinition("ShippingProviderPart", part => part
                 .Attachable()
                 .WithDescription("Allows your content item to contain ShippingOptions."));

            ContentDefinitionManager.AlterTypeDefinition("ShippingProvider", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ShippingProviderPart")
                .WithPart("TitlePart")
                );

            return 1;
        }

    }
}