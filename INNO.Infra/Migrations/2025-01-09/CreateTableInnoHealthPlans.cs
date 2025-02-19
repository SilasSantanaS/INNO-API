using FluentMigrator;
using System.Data;

namespace INNO.Infra.Migrations
{
    [Migration(4)]
    public class CreateTableInnoHealthPlans : Migration
    {
        private readonly string tableName = "inno_health_plans";
        private readonly string innoTenants = "inno_tenants";

        private readonly string name = "name";
        private readonly string tenantId = "tenant_id";
        private readonly string createdAt = "created_at";
        private readonly string updatedAt = "updated_at";
        private readonly string inactivatedAt = "inactivated_at";

        public override void Up()
        {
            if (!Schema.Table(tableName).Exists())
            {
                Create.Table(tableName)
                    .WithColumn("id")
                        .AsCustom("SERIAL")
                        .PrimaryKey()
                    .WithColumn(name)
                        .AsString(50)
                        .NotNullable()
                    .WithColumn(tenantId)
                        .AsInt32()
                        .NotNullable()
                        .ForeignKey(innoTenants, "id").OnDelete(Rule.Cascade)
                    .WithColumn(createdAt)
                        .AsDateTime()
                        .WithDefault(SystemMethods.CurrentDateTime)
                    .WithColumn(updatedAt)
                        .AsDateTime()
                        .Nullable()
                    .WithColumn(inactivatedAt)
                        .AsDateTime()
                        .Nullable();
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
