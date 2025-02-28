using FluentMigrator;
using System.Data;

namespace INNO.Infra.Migrations
{
    [Migration(4)]
    public class CreateTableInnoAddresses : Migration
    {
        private readonly string tableName = "inno_addresses";
        private readonly string innoTenants = "inno_tenants";

        private readonly string city = "city";
        private readonly string state = "state";
        private readonly string street = "street";
        private readonly string number = "number";
        private readonly string zipCode = "zip_code";
        private readonly string tenantId = "tenant_id";
        private readonly string complement = "complement";
        private readonly string neighborhood = "neighborhood";

        public override void Up()
        {
            if (!Schema.Table(tableName).Exists())
            {
                Create.Table(tableName)
                    .WithColumn("id")
                        .AsCustom("SERIAL")
                        .PrimaryKey()
                    .WithColumn(city)
                        .AsString(50)
                        .Nullable()
                    .WithColumn(state)
                        .AsString(2)
                        .Nullable()
                    .WithColumn(street)
                        .AsString(80)
                        .Nullable()
                    .WithColumn(number)
                        .AsString(10)
                        .Nullable()
                    .WithColumn(zipCode)
                        .AsString(9)
                        .Nullable()
                    .WithColumn(complement)
                        .AsString(50)
                        .Nullable()
                    .WithColumn(neighborhood)
                        .AsString(100)
                        .Nullable()
                    .WithColumn(tenantId)
                        .AsInt32()
                        .NotNullable()
                        .ForeignKey(innoTenants, "id").OnDelete(Rule.Cascade);
            }
        }

        public override void Down()
        {
            if (Schema.Table(tableName).Exists())
            {
                Delete.Table(tableName);
            }
        }
    }
}
