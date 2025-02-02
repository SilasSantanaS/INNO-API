using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using Microsoft.Extensions.Options;

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
                "created_at",
                "inactivated_at",
            ];
        }

        public async Task<Patient> CreatePatient(Patient data)
        {
            var qry = $@"INSERT INTO {tableName} (
                            name
                         ) VALUES (
                            @{nameof(data.Name)}
                         ) RETURNING {GetCols()};";

            return await QueryFirstAsync<Patient>(qry, data);
        }

        public Task<bool> DeletePatient(int id, int tenantId)
        {
            throw new NotImplementedException();
        }

        public async Task<Patient> GetPatientById(int? id)
        {
            var qry = $@"SELECT 
                            {GetCols("p")}
                         FROM 
                            {tableName} AS p
                         WHERE 
                            p.id = @ID
                         LIMIT 1;";

            return await QueryFirstAsync<Patient>(qry, new
            {
                ID = id,
            });
        }

        public async Task<int> GetTotalItems(PatientFilter filter)
        {
            var qry = $@"SELECT
                            count(*)
                         FROM
                            {tableName}
                         WHERE
                            {BuildFilter(filter)};";

            return await QueryFirstAsync<int?>(qry, filter) ?? 0;
        }

        public async Task<IEnumerable<Patient>> ListPatients(PatientFilter filter)
        {
            var offset = Paginate(filter.GetPage(), filter.GetPageLimit());

            var qry = $@"SELECT
                            {GetCols()}
                         FROM
                            {tableName}
                         WHERE
                            {BuildFilter(filter)}
                         LIMIT 
                            {filter.GetPageLimit()} OFFSET {offset};";

            return await QueryAsync<Patient>(qry, filter);
        }

        public Task<Patient> UpdatePatient(int? id, Patient data)
        {
            throw new NotImplementedException();
        }

        private string BuildFilter(PatientFilter filter)
        {
            var result = new List<string>();

            if (filter.TenantId.HasValue)
            {
                result.Add($@"tenant_id = @{nameof(filter.TenantId)}");
            }

            return string.Join(" AND ", result);
        }
    }
}
