using Dapper;
using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using System.Data;

namespace INNO.Infra.Repositories
{
    public class PatientRepository : Repository, IPatientRepository
    {
        public PatientRepository(
            IDbConnectionFactory dbConnectionFactory,
            CurrentSession session,
            IOptionsSnapshot<DbSettings> settings
        ) : base(dbConnectionFactory, session, settings)
        {
            tableName = "inno_patients";
            tableCols = [
                "id",
                "name",
                "user_id",
                "tenant_id",
                "contact_id",
                "address_id",
                "created_at",
                "updated_at",
                "mother_name",
                "father_name",
                "date_of_birth",
                "health_plan_id",
                "inactivated_at"
            ];
        }

        public async Task<bool> ActivatePatient(int? id, int? tenantId)
        {
            if (id == null || tenantId == null)
            {
                return false;
            }

            var qry = $@"UPDATE {tableName} SET
                            inactivated_at = null
                         WHERE
                            id = @ID
                            AND tenant_id = @TENANTID";

            return await ExecuteAsync(qry, new
            {
                ID = id,
                TENANTID = tenantId
            }) > 0;
        }

        public async Task<Patient> CreatePatient(Patient data)
        {
            var qry = $@"INSERT INTO {tableName} (
                            name,
                            user_id,
                            tenant_id,
                            address_id,
                            contact_id,
                            mother_name,
                            father_name,   
                            date_of_birth,
                            health_plan_id
                         ) VALUES (
                            @{nameof(data.Name)},
                            @{nameof(data.UserId)},
                            @{nameof(data.TenantId)},
                            @{nameof(data.AddressId)},
                            @{nameof(data.ContactId)},
                            @{nameof(data.MotherName)},
                            @{nameof(data.FatherName)},
                            @{nameof(data.DateOfBirth)},
                            @{nameof(data.HealthPlanId)}
                         ) RETURNING {GetCols()};";

            return await QueryFirstAsync<Patient>(qry, data);
        }

        public async Task<Patient?> GetPatientById(int? id, PatientFilter filter)
        {
            if (id == null)
            {
                return null;
            }

            filter.Id = id;

            var qry = $@"SELECT 
                            p.*,
                            a.*,
                            c.*
                         FROM 
                            {tableName} AS p
                         LEFT JOIN 
                            inno_contacts AS c ON c.id = p.contact_id
                         LEFT JOIN
                            inno_addresses AS a ON a.id = p.address_id
                         WHERE 
                            {BuildFilter(filter)}
                         LIMIT 1;";

            return (await SelectPatients(qry, filter)).Values.FirstOrDefault();
        }

        public async Task<int> GetTotalItems(PatientFilter filter)
        {
            var qry = $@"SELECT
                            count(p.*)
                         FROM
                            {tableName} AS p
                         WHERE
                            {BuildFilter(filter)};";

            return await QueryFirstAsync<int?>(qry, filter) ?? 0;
        }

        public async Task<bool> InactivatePatient(int? id, int? tenantId)
        {
            if (id == null || tenantId == null)
            {
                return false;
            }

            var qry = $@"UPDATE {tableName} SET
                            inactivated_at = current_timestamp
                         WHERE
                            id = @ID
                            AND tenant_id = @TENANTID";

            return await ExecuteAsync(qry, new
            {
                ID = id,
                TENANTID = tenantId
            }) > 0;
        }

        public async Task<IEnumerable<Patient>> ListPatients(PatientFilter filter)
        {
            var offset = Paginate(filter.GetPage(), filter.GetPageLimit());

            var qry = $@"SELECT
                            p.*,
                            a.*,
                            c.*
                         FROM
                            {tableName} AS p
                         LEFT JOIN 
                            inno_contacts AS c ON c.id = p.contact_id
                         LEFT JOIN
                            inno_addresses AS a ON a.id = p.address_id
                         WHERE
                            {BuildFilter(filter)}
                         LIMIT 
                            {filter.GetPageLimit()} OFFSET {offset};";


            return (await SelectPatients(qry, filter)).Values;
        }

        public async Task<Patient?> UpdatePatient(int? id, Patient data)
        {
            if (id == null)
            {
                return null;
            }

            var updatedCols = new List<string>();

            if (!string.IsNullOrWhiteSpace(data.Name))
            {
                updatedCols.Add($"name = @{nameof(data.Name)}");
            }

            if (!string.IsNullOrWhiteSpace(data.MotherName))
            {
                updatedCols.Add($"mother_name = @{nameof(data.MotherName)}");
            }

            if (!string.IsNullOrWhiteSpace(data.FatherName))
            {
                updatedCols.Add($"father_name = @{nameof(data.FatherName)}");
            }

            if(data.UserId.HasValue)
            {
                updatedCols.Add($"user_id = @{nameof(data.UserId)}");
            }

            if (data.DateOfBirth.HasValue)
            {
                updatedCols.Add($"date_of_birth = @{nameof(data.DateOfBirth)}");
            }

            updatedCols.Add($"updated_at = current_timestamp");

            var qry = $@"UPDATE 
                            {tableName} 
                         SET 
                            {string.Join(",", updatedCols)}
                         WHERE 
                            id = @{nameof(data.Id)}                             
                            AND tenant_id = @{nameof(data.TenantId)} 
                         RETURNING {GetCols()};";

            data.Id = id.Value;

            return await QueryFirstAsync<Patient>(qry, data);
        }

        private async Task<IDictionary<int, Patient>> SelectPatients(string qry, PatientFilter filter, string splitOn = "id")
        {
            using IDbConnection connection = _connectionFactory.GetConnection();

            var resultDictionary = new Dictionary<int, Patient>();

            connection.Open();

            await connection.QueryAsync<Patient, Address, Contact, Patient>(
                qry,
                (patient, address, contact) =>
                {
                    if (!resultDictionary.TryGetValue(patient.Id, out var patientEntry))
                    {
                        patientEntry = patient;

                        resultDictionary.Add(patientEntry.Id, patientEntry);
                    }

                    if (contact != null)
                    {
                        patientEntry.Contact = contact;
                    }

                    if (address != null)
                    {
                        patientEntry.Address = address;
                    }

                    return patientEntry;
                },
                filter,
                splitOn: splitOn
            );

            return resultDictionary;
        }

        private string BuildFilter(PatientFilter filter)
        {
            var result = new List<string>()
            {
                $@"p.tenant_id = @{nameof(filter.TenantId)}"
            };

            if (filter.Id.HasValue)
            {
                result.Add($@"p.id = @{nameof(filter.Id)}");
            }

            if (filter.Inactive ?? false)
            {
                result.Add($@"p.inactivated_at IS NOT NULL");
            }
            else
            {
                result.Add($@"p.inactivated_at IS NULL");
            }

            return string.Join(" AND ", result);
        }
    }
}
