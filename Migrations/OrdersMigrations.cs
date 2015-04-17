using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace OShop.Migrations {
    [OrchardFeature("OShop.Orders")]
    public class OrdersMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("OrderAddressRecord", table => table
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<string>("Company")
                .Column<string>("FirstName")
                .Column<string>("LastName")
                .Column<string>("Address1")
                .Column<string>("Address2")
                .Column<string>("Zipcode")
                .Column<string>("City")
                .Column<int>("LocationsCountryRecord_Id", c => c.NotNull())
                .Column<int>("LocationsStateRecord_Id", c => c.Nullable())
            );

            SchemaBuilder.CreateTable("OrderPartRecord", table => table
                .ContentPartRecord()
                .Column<string>("Reference", c => c.WithLength(16).Unique())
                .Column<int>("BillingAddressId")
                .Column<int>("OrderStatus")
                .Column<decimal>("OrderTotal")
                .Column<string>("Logs", c => c.Unlimited())
            );

            SchemaBuilder.CreateTable("OrderDetailRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<int>("OrderId")
                 .Column<string>("DetailType")
                 .Column<int>("ContentId")
                 .Column<string>("SKU")
                 .Column<string>("Designation")
                 .Column<string>("Description")
                 .Column<decimal>("UnitPrice")
                 .Column<int>("Quantity")
                 .Column<decimal>("ReductionPercent")
                 .Column<decimal>("ReductionAmount")
                 .Column<string>("Data", c => c.Unlimited())
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