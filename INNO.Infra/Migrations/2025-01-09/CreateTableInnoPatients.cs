using FluentMigrator;
using System.Data;

namespace INNO.Infra.Migrations
{
    [Migration(8)]
    public class CreateTableInnoPatients : Migration
    {
        private readonly string tableName = "inno_patients";

        private readonly string innoUsers = "inno_users";
        private readonly string innoTenants = "inno_tenants";
        private readonly string innoContacts = "inno_contacts";
        private readonly string innoAddresses = "inno_addresses";
        private readonly string innoHealthPlans = "inno_health_plans";

        private readonly string name = "name";
        private readonly string userId = "user_id";
        private readonly string tenantId = "tenant_id";
        private readonly string contactId = "contact_id";
        private readonly string addressId = "address_id";
        private readonly string createdAt = "created_at";
        private readonly string updatedAt = "updated_at";
        private readonly string motherName = "mother_name";
        private readonly string fatherName = "father_name";
        private readonly string dateOfBirth = "date_of_birth";
        private readonly string healthPlanId = "health_plan_id";
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
                        .AsString(255)
                        .NotNullable()
                    .WithColumn(motherName)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(fatherName)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(dateOfBirth)
                        .AsDate()
                        .Nullable()
                    .WithColumn(healthPlanId)
                        .AsInt32()
                        .Nullable()
                        .ForeignKey(innoHealthPlans, "id").OnDelete(Rule.Cascade)
                    .WithColumn(userId)
                        .AsInt32()
                        .Nullable()
                        .ForeignKey(innoUsers, "id").OnDelete(Rule.Cascade)
                    .WithColumn(contactId)
                        .AsInt32()
                        .Nullable()
                        .ForeignKey(innoContacts, "id").OnDelete(Rule.Cascade)
                    .WithColumn(addressId)
                        .AsInt32()
                        .Nullable()
                        .ForeignKey(innoAddresses, "id").OnDelete(Rule.Cascade)
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
