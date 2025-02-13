using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using System.Data;

namespace INNO.Infra.Repositories
{
    public class TenantPreferencesRepository : Repository, ITenantPreferencesRepository
    {
        public TenantPreferencesRepository(
            IDbConnectionFactory dbConnectionFactory,
            CurrentSession session,
            IOptionsSnapshot<DbSettings> settings
        ) : base(dbConnectionFactory, session, settings)
        {
            tableName = "inno_settings";
            tableCols =
            [
                "id",
                "token_duration",
                "invite_duration",
                "tenant_id"
            ];
        }

        public async Task<TenantPreferences> CreatePreferences(TenantPreferences data, IDbTransaction transaction = null)
        {
            var qry = $@"INSERT INTO {tableName} (
                            token_duration,
                            invite_duration,
                            tenant_id
                         ) VALUES (
                            @{nameof(data.TokenDuration)},
                            @{nameof(data.InviteDuration)},
                            @{nameof(data.TenantId)}
                         ) RETURNING *;";

            return await QueryFirstAsync<TenantPreferences>(qry, data, transaction: transaction);
        }

        public async Task<TenantPreferences> GetPreferences(int? tenantId)
        {
            if (tenantId == null)
            {
                return null;
            }

            var qry = $@"SELECT 
                            * 
                         FROM 
                            {tableName}
                         WHERE
                            tenant_id = @TENANTID
                         LIMIT 1;";

            return await QueryFirstAsync<TenantPreferences>(qry, new
            {
                TENANTID = tenantId,
            }) ?? new TenantPreferences();
        }

        public async Task<TenantPreferences> UpdatePreferences(int? tenantId, TenantPreferences data)
        {
            if (tenantId == null)
            {
                return null;
            }

            var updatedCols = new List<string>();

            if (data.TokenDuration.HasValue)
            {
                updatedCols.Add($"token_duration = @{nameof(data.TokenDuration)}");
            }

            if (data.InviteDuration.HasValue)
            {
                updatedCols.Add($"invite_duration = @{nameof(data.InviteDuration)}");
            }

            if(!updatedCols.Any())
            {
                return data;
            }

            var qry = $@"UPDATE {tableName} SET
                            {string.Join(",", updatedCols)}
                         WHERE
                            tenant_id = @{nameof(data.TenantId)}
                         RETURNING *;";

            data.TenantId = tenantId.Value;

            return await QueryFirstAsync<TenantPreferences>(qry, data);
        }
    }
}
