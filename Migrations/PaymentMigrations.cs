using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using System;

namespace OShop.Migrations {
    [OrchardFeature("OShop.Payment")]
    public class PaymentMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("PaymentPartRecord", table => table
                .ContentPartRecord()
            );

            SchemaBuilder.CreateTable("PaymentTransactionRecord", table => table
                 .Column<int>("Id", c => c.PrimaryKey().Identity())
                 .Column<int>("PaymentPartRecord_Id", c => c.NotNull())
                 .Column<DateTime>("Date")
                 .Column<decimal>("Amount")
                 .Column<string>("Method")
                 .Column<string>("TransactionId")
                 .Column<string>("Status", c => c.WithLength(16))
                 .Column<string>("Data", c => c.Unlimited())
            );

            SchemaBuilder.CreateForeignKey("FK_PaymentTransactionRecord_PaymentPartRecord", "PaymentTransactionRecord", new[] { "PaymentPartRecord_Id" }, "PaymentPartRecord", new[] { "Id" });

            ContentDefinitionManager.AlterTypeDefinition("Order", type => type
                .WithPart("PaymentPart")
            );

            return 1;
        }
    }
}