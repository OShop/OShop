using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using System;

namespace OShop.Migrations {
    [OrchardFeature("OShop.Customers")]
    public class CustomerMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("CustomerPartRecord", table => table
                 .ContentPartRecord()
                 .Column<int>("UserId")
                 .Column<string>("FirstName")
                 .Column<string>("LastName")
                 .Column<int>("DefaultAddressId"));

            SchemaBuilder.CreateTable("CustomerAddressPartRecord", table => table
                 .ContentPartRecord()
                 .Column<int>("CustomerId", c => c.NotNull())
                 .Column<string>("AddressAlias")
                 .Column<string>("Company")
                 .Column<string>("FirstName")
                 .Column<string>("LastName")
                 .Column<string>("Address1")
                 .Column<string>("Address2")
                 .Column<string>("Zipcode")
                 .Column<string>("City")
                 .Column<int>("LocationsCountryId")
                 .Column<int>("LocationsStateId"));

            SchemaBuilder.CreateTable("CustomerOrderPartRecord", table => table
                 .ContentPartRecord()
                 .Column<int>("CustomerId")
                 );

            ContentDefinitionManager.AlterPartDefinition("CustomerPart", part => part
                .Attachable(false)
                );

            ContentDefinitionManager.AlterTypeDefinition("Customer", type => type
                .WithPart("CommonPart")
                .WithPart("CustomerPart")
                .Creatable(false)
                );

            ContentDefinitionManager.AlterPartDefinition("CustomerAddressPart", part => part
                .Attachable(false)
                );

            ContentDefinitionManager.AlterTypeDefinition("CustomerAddress", type => type
                .WithPart("CommonPart")
                .WithPart("CustomerAddressPart")
                .Creatable(false)
                );

            ContentDefinitionManager.AlterPartDefinition("CustomerOrderPart", part => part
                .Attachable(false)
                );

            ContentDefinitionManager.AlterTypeDefinition("Order", type => type
                .WithPart("CustomerOrderPart")
            );

            return 1;
        }

    }
}