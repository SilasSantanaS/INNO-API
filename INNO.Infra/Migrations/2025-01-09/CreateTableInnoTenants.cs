using FluentMigrator;

namespace INNO.Infra.Migrations
{
    [Migration(0)]
    public class CreateTableInnoTenants : Migration
    {
        private readonly string tableName = "inno_tenants";

        private readonly string name = "name";
        private readonly string createdAt = "created_at";
        private readonly string updatedAt = "updated_at";
        private readonly string inactivatedAt = "inactivated_at";
        private readonly string corporateName = "corporate_name";
        private readonly string pricingTierId = "pricing_tier_id";

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
                    .WithColumn(corporateName)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(pricingTierId)
                        .AsInt32()
                        .Nullable()
                    .WithColumn(createdAt)
                        .AsDateTime()
                        .NotNullable()
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
