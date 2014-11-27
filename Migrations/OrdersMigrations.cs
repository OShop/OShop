using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace OShop.Migrations {
    [OrchardFeature("OShop.Orders")]
    public class OrdersMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("OrderPartRecord", table => table
                .ContentPartRecord()
                .Column<string>("Reference", c => c.WithLength(16).Unique())
                .Column<string>("CustomerInfos", c => c.Unlimited())
                .Column<string>("BillingAddress", c => c.Unlimited())
                .Column<int>("OrderStatus")
                .Column<string>("Items", c => c.Unlimited())
                .Column<string>("Logs", c => c.Unlimited())
            );

            ContentDefinitionManager.AlterTypeDefinition("Order", type => type
                .WithPart("CommonPart")
                .WithPart("OrderPart")
                .Creatable(false)
                .Draftable(false)
            );

            return 1;
        }
    }
}