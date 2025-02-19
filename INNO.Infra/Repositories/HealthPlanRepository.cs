using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace INNO.Infra.Repositories
{
    public class HealthPlanRepository : Repository, IHealthPlanRepository
    {
        public HealthPlanRepository(
            IDbConnectionFactory dbConnectionFactory,
            CurrentSession session,
            IOptionsSnapshot<DbSettings> settings
        ) : base(dbConnectionFactory, session, settings)
        {
            tableName = "inno_health_plans";
            tableCols = [
                "id",
                "name",
                "created_at",
                "updated_at",
                "inactivated_at",
            ];
        }

        public async Task<HealthPlan> CreateHealthPlan(HealthPlan data)
        {
            var qry = $@"INSERT INTO {tableName} (
                            name,
                            tenant_id
                         ) VALUES (
                            @{nameof(data.Name)},                            
                            @{nameof(data.TenantId)}
                         ) RETURNING {GetCols()};";

            return await QueryFirstAsync<HealthPlan>(qry, data);
        }

        public async Task<bool> DeleteHealthPlan(int id, int tenantId)
        {
            var qry = $@"DELETE FROM 
                            {tableName} 
                          WHERE 
                            id = @ID 
                            AND tenant_id = @TENANTID;";

            return await ExecuteAsync(qry, new
            {
                ID = id,
                TENANTID = tenantId,
            }) > 0;
        }

        public async Task<HealthPlan?> GetHealthPlanById(int? id, int? tenantId)
        {
            if(id == null || tenantId == null)
            {
                return null;
            }

            var qry = $@"SELECT 
                            {GetCols()}
                         FROM 
                            {tableName}
                         WHERE 
                            id = @ID
                            AND tenant_id = @TENANTID;";

            return await QueryFirstAsync<HealthPlan>(qry, new
            {
                ID = id,
                TENANTID = tenantId
            });
        }

        public async Task<int> GetTotalItems(HealthPlanFilter filter)
        {
            var where = BuildFilter(filter);
            where = string.IsNullOrWhiteSpace(where) ? null : $"WHERE {where}";

            var qry = $@"SELECT
                            count(*)
                         FROM
                            {tableName}
                         {where};";

            return await QueryFirstAsync<int?>(qry, filter) ?? 0;
        }

        public async Task<IEnumerable<HealthPlan>> ListHealthPlans(HealthPlanFilter filter)
        {
            var offset = Paginate(filter.GetPage(), filter.GetPageLimit());

            var where = BuildFilter(filter);
            where = string.IsNullOrWhiteSpace(where) ? null : $"WHERE {where}";

            var qry = $@"SELECT
                            {GetCols()}
                         FROM
                            {tableName}
                         {where}
                         LIMIT 
                            {filter.GetPageLimit()} OFFSET {offset};";

            return await QueryAsync<HealthPlan>(qry, filter);
        }

        public async Task<HealthPlan> UpdateHealthPlan(int? id, HealthPlan data)
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

            return await QueryFirstAsync<HealthPlan>(qry, data);
        }

        private string BuildFilter(HealthPlanFilter filter)
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
