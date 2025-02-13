using FluentMigrator;
using System.Data;

namespace INNO.Infra.Migrations
{
    [Migration(3)]
    public class CreateTableInnoSettings : Migration
    {
        private readonly string tableName = "inno_settings";
        private readonly string innoTenants = "inno_tenants";

        private readonly string tenantId = "tenant_id";
        private readonly string tokenDuration = "token_duration";
        private readonly string inviteDuration = "invite_duration";
        
        public override void Up()
        {
            if (!Schema.Table(tableName).Exists())
            {
                Create.Table(tableName)
                    .WithColumn("id")
                        .AsCustom("SERIAL")
                        .PrimaryKey()
                    .WithColumn(tokenDuration)
                        .AsInt32()
                        .NotNullable()
                        .WithDefaultValue(2)
                    .WithColumn(inviteDuration)
                        .AsInt32()
                        .NotNullable()
                        .WithDefaultValue(2)
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
