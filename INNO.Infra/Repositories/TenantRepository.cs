using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using System.Data;

namespace INNO.Infra.Repositories
{
    public class TenantRepository : Repository, ITenantRepository
    {
        public TenantRepository(
            IDbConnectionFactory dbConnectionFactory, 
            CurrentSession session, 
            IOptionsSnapshot<DbSettings> settings
        ) : base(dbConnectionFactory, session, settings)
        {
            tableName = "inno_tenants";
            tableCols = [
                "id",
                "name",
                "created_at",
                "corporate_name",
                "inactivated_at",
                "pricing_tier_id",
            ];
        }

        public async Task<bool> ActivateTenant(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var qry = $@"UPDATE {tableName} SET
                            inactivated_at = null
                         WHERE
                            id = @ID";

            return await ExecuteAsync(qry, new
            {
                ID = id
            }) > 0;
        }

        public async Task<Tenant> CreateTenant(Tenant data, IDbTransaction? transaction = null)
        {
            var qry = $@"INSERT INTO {tableName}(
                            name,
                            corporate_name,
                            pricing_tier_id,
                            created_at                            
                         ) VALUES (
                            @{nameof(data.Name)},                            
                            @{nameof(data.CorporateName)},
                            @{nameof(data.PricingTierId)},
                            current_timestamp
                         ) RETURNING *;";

            return await QueryFirstAsync<Tenant>(qry, data, transaction: transaction);
        }

        public async Task<Tenant?> GetTenantById(int? id)
        {
            if(id == null)
            {
                return null;
            }

            var qry = $@"SELECT
                            *
                         FROM
                            {tableName} AS t
                         WHERE
                            t.id = @id;";

            return await QueryFirstAsync<Tenant?>(qry, new { id });
        }

        public async Task<Tenant?> GetTenantByName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var filter = new Tenant()
            {
                Name = name
            };

            var qry = $@"SELECT
                            *
                         FROM
                            {tableName} AS t
                         WHERE
                            t.name = @{nameof(filter.Name)};";

            return await QueryFirstAsync<Tenant?>(qry, filter);
        }

        public async Task<int> GetTotalItems(TenantFilter filter)
        {
            var qry = $@"SELECT
                            count(*)
                         FROM
                            {tableName} AS t
                         WHERE
                            {BuildFilter(filter)};";

            return await QueryFirstAsync<int?>(qry, filter) ?? 0;
        }

        public async Task<bool> InactivateTenant(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var qry = $@"UPDATE {tableName} SET
                            inactivated_at = current_timestamp
                         WHERE
                            id = @ID";

            return await ExecuteAsync(qry, new
            {
                ID = id
            }) > 0;
        }

        public async Task<IEnumerable<Tenant>> ListTenants(TenantFilter filter)
        {
            var offset = Paginate(filter.GetPage(), filter.GetPageLimit());

            var qry = $@"SELECT
                            {GetCols("t")}
                         FROM
                            {tableName} AS t
                         WHERE
                            {BuildFilter(filter)}
                         LIMIT
                            {filter.GetPageLimit()} OFFSET {offset};";

            return await QueryAsync<Tenant>(qry, filter);
        }

        public  async Task<Tenant> UpdateTenant(int? id, Tenant data)
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

            if (!string.IsNullOrWhiteSpace(data.CorporateName))
            {
                updatedCols.Add($"corporate_name = @{nameof(data.CorporateName)}");
            }

            if (data.PricingTierId.HasValue)
            {
                updatedCols.Add($"pricing_tier_id = @{nameof(data.PricingTierId)}");
            }

            updatedCols.Add($"updated_at = current_timestamp");

            var qry = $@"UPDATE 
                            {tableName} 
                         SET 
                            {string.Join(",", updatedCols)}
                         WHERE 
                            id = @{nameof(data.Id)} 
                         RETURNING {GetCols()};";

            data.Id = id.Value;

            return await QueryFirstAsync<Tenant>(qry, data);
        }

        private string BuildFilter(TenantFilter filter)
        {
            var result = new List<string>();

            if (filter.InactivatedAfter.HasValue)
            {
                result.Add($@"t.inactivated_at > @{nameof(filter.InactivatedAfter)}");
            }
            else if (filter.Inactive)
            {
                result.Add($@"t.inactivated_at IS NOT NULL");
            }
            else
            {
                result.Add($@"t.inactivated_at IS NULL");
            }

            if (filter.TenantId.HasValue)
            {
                result.Add($@"t.id = @{nameof(filter.TenantId)}");
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                filter.Search = $"{filter.Search.ToUpper()}%";

                result.Add($@"UPPER(t.name) LIKE @{nameof(filter.Search)} OR UPPER(t.hostname) LIKE @{nameof(filter.Search)}");
            }

            return string.Join(" AND ", result);
        }
    }
}
