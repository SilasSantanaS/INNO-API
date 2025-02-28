using FluentMigrator;
using System.Data;

namespace INNO.Infra.Migrations
{
    [Migration(5)]
    public class CreateTableInnoContacts : Migration
    {
        private readonly string tableName = "inno_contacts";
        private readonly string innoTenants = "inno_tenants";

        private readonly string obs = "obs";
        private readonly string name = "name";
        private readonly string phone = "phone";
        private readonly string email = "email";
        private readonly string tenantId = "tenant_id";

        public override void Up()
        {
            if (!Schema.Table(tableName).Exists())
            {
                Create.Table(tableName)
                    .WithColumn("id")
                        .AsCustom("SERIAL")
                        .PrimaryKey()
                    .WithColumn(obs)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(name)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(phone)
                        .AsString(20)
                        .Nullable()
                    .WithColumn(email)
                        .AsString(70)
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
