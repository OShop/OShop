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
            return 1;
        }
    }
}