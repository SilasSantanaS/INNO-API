using FluentMigrator;
using System.Data;

namespace INNO.Infra.Migrations
{
    [Migration(9)]
    public class CreateTableInnoPatientAttachments : Migration
    {
        private readonly string tableName = "inno_patient_attachments";

        private readonly string innoTenants = "inno_tenants";
        private readonly string innoPatients = "inno_patients";

        private readonly string tenantId = "tenant_id";
        private readonly string fileName = "file_name";
        private readonly string createdAt = "created_at";
        private readonly string patientId = "patient_id";
        private readonly string description = "description";
        private readonly string storageHost = "storage_host";
        private readonly string exhibitionOrder = "exhibition_order";

        public override void Up()
        {
            if (!Schema.Table(tableName).Exists())
            {
                Create.Table(tableName)
                    .WithColumn("id")
                        .AsCustom("SERIAL")
                        .PrimaryKey()
                   .WithColumn(tenantId)
                        .AsInt32()
                        .Nullable()
                        .ForeignKey(innoTenants, "id").OnDelete(Rule.Cascade)
                    .WithColumn(patientId)
                        .AsInt32()
                        .ForeignKey(innoPatients, "id")
                    .WithColumn(fileName)
                        .AsString(500)
                        .NotNullable()
                    .WithColumn(exhibitionOrder)
                        .AsInt32()
                        .Nullable()
                    .WithColumn(description)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(storageHost)
                        .AsString(255)
                        .NotNullable()
                    .WithColumn(createdAt)
                        .AsDateTime()
                        .WithDefault(SystemMethods.CurrentDateTime);
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
