using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace OShop.Migrations {
    [OrchardFeature("OShop.Products")]
    public class ProductsMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("ProductPartRecord", table => table
                 .ContentPartVersionRecord()
                 .Column<decimal>("UnitPrice", c => c.NotNull())
                 .Column<string>("SKU")
                 .Column<int>("VatRecord_Id"));

            ContentDefinitionManager.AlterPartDefinition("ProductPart", part => part
                 .Attachable()
                 .WithDescription("Allows content to be sold as a Product"));
            return 1;
        }
    }
}