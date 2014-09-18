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
                 .Column<double>("Width")
                 .Column<double>("Height")
                 .Column<double>("Lenght"));

            ContentDefinitionManager.AlterPartDefinition("ShippingPart", part => part
                 .Attachable()
                 .WithDescription("Add shipping information to your products"));

            SchemaBuilder.CreateTable("ShippingZoneRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<bool>("Enabled")
                 .Column<string>("Name"));

            return 2;
        }

        public int UpdateFrom1() {
            SchemaBuilder.CreateTable("ShippingZoneRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<bool>("Enabled")
                 .Column<string>("Name"));

            return 2;
        }
    }
}