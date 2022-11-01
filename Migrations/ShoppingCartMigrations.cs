using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using System;

namespace OShop.Migrations {
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("ShoppingCartRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<string>("Guid", c => c.WithLength(36))
                 .Column<DateTime>("ModifiedUtc")
                 .Column<int>("OwnerId", c => c.Nullable())
                 .Column<string>("Data", c => c.Unlimited()));

            SchemaBuilder.CreateTable("ShoppingCartItemRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<int>("ShoppingCartRecord_Id")
                 .Column<string>("ItemType")
                 .Column<int>("ItemId", c => c.NotNull())
                 .Column<int>("Quantity"));

            SchemaBuilder.CreateForeignKey("FK_ShoppingCartItemRecord_ShoppingCartRecord", "ShoppingCartItemRecord", new[] { "ShoppingCartRecord_Id" }, "ShoppingCartRecord", new[] { "Id" });

            ContentDefinitionManager.AlterTypeDefinition("ShoppingCartWidget", type => type
                .WithPart("CommonPart")
                .WithPart("ShoppingCartWidgetPart")
                .WithPart("WidgetPart")
                .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.AlterTable("ShoppingCartRecord", table => table
                .CreateIndex("IDX_ShoppingCartRecord_Guid", "Guid")
            );

            return 2;
        }
    }
}