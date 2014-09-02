using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace OShop.Migrations {
    [OrchardFeature("OShop.VAT")]
    public class VatMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("VatRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<string>("Name")
                 .Column<decimal>("Rate"));

            return 1;
        }

    }
}