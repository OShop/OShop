using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Migrations {
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("ShoppingCartRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<string>("Guid", c => c.WithLength(36))
                 .Column<DateTime>("ModifiedUtc")
                 .Column<int>("OwnerId"));

            SchemaBuilder.CreateTable("ShoppingCartItem", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<int>("ShoppingCartRecord_Id", c => c.NotNull())
                 .Column<string>("ItemType")
                 .Column<int>("ItemId", c => c.NotNull())
                 .Column<int>("Quantity"));

            SchemaBuilder.CreateForeignKey("FK_ShoppingCartItem_ShoppingCartRecord", "ShoppingCartItem", new[] { "ShoppingCartRecord_Id" }, "ShoppingCartRecord", new[] { "Id" });

            return 1;
        }
    }
}