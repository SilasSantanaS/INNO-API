using FluentMigrator;
using System.Data;

namespace INNO.Infra.Migrations
{
    [Migration(2)]
    public class CreateTableInnoUsers : Migration
    {
        private readonly string tableName = "inno_users";
        private readonly string innoTenants = "inno_tenants";

        private readonly string name = "name";
        private readonly string email = "email";
        private readonly string document = "document";
        private readonly string password = "password";
        private readonly string tenantId = "tenant_id";
        private readonly string profileId = "profile_id";
        private readonly string createdAt = "created_at";
        private readonly string updatedAt = "updated_at";
        private readonly string refreshToken = "refresh_token";
        private readonly string inactivatedAt = "inactivated_at";
        private readonly string refrestTokenExpiresAt = "refresh_token_expires_at";

        public override void Up()
        {
            if (!Schema.Table(tableName).Exists())
            {
                Create.Table(tableName)
                    .WithColumn("id")
                        .AsCustom("SERIAL")
                        .PrimaryKey()
                    .WithColumn(email)
                        .AsString(255)
                        .NotNullable()
                    .WithColumn(password)
                        .AsString(255)
                        .NotNullable()
                    .WithColumn(name)
                        .AsString(50)
                        .NotNullable()
                    .WithColumn(document)
                        .AsString(20)
                        .Nullable()
                    .WithColumn(tenantId)
                        .AsInt32()
                        .Nullable()
                        .ForeignKey(innoTenants, "id").OnDelete(Rule.Cascade)
                    .WithColumn(refreshToken)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(refrestTokenExpiresAt)
                        .AsDateTime()
                        .Nullable()
                    .WithColumn(profileId)
                        .AsInt32()
                        .NotNullable()
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
