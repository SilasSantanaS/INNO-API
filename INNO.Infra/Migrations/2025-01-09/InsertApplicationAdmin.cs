using FluentMigrator;
using INNO.Domain.Enums;
using INNO.Domain.Helpers;

namespace INNO.Infra.Migrations
{
    [Migration(2)]
    public class InsertApplicationAdmin : Migration
    {
        private readonly string tableName = "inno_users";
        public override void Up()
        {
            if (Schema.Table(tableName).Exists())
            {
                Insert.IntoTable(tableName)
                .Row(new
                {
                    email = "app-admin@inno.com",
                    name = "Administrador da Aplicação",
                    password = PasswordHelper.GetHash("kdjaokdjfos-0"),
                    profile_id = (int)EUserProfile.ApplicationManager,
                });
            }
        }

        public override void Down()
        {
            if (Schema.Table(tableName).Exists())
            {
                Execute.Sql($@"DELETE FROM {tableName} WHERE email = 'app-admin@inno.com';");
            }
        }
    }
}
