using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace OShop.Migrations {
    [OrchardFeature("OShop.Stocks")]
    public class StocksMigration : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("StockPartRecord", table => table
                .ContentPartRecord()
                .Column<bool>("EnableStockMgmt", c => c.NotNull())
                .Column<int>("InStockQty", c => c.NotNull())
                .Column<int>("InOrderQty", c => c.NotNull())
                .Column<int>("AlertQty", c => c.NotNull())
                .Column<bool>("AllowOutOfStock", c => c.NotNull())
            );

            ContentDefinitionManager.AlterPartDefinition("StockPart", part => part
                .Attachable()
                .WithDescription("Enables stock management on content items")
            );

            return 1;
        }
    }
}