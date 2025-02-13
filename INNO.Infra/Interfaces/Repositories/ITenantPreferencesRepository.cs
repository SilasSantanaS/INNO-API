using INNO.Domain.Settings;
using System.Data;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface ITenantPreferencesRepository
    {
        Task<TenantPreferences> GetPreferences(int? tenantId);
        Task<TenantPreferences> UpdatePreferences(int? tenantId, TenantPreferences data);
        Task<TenantPreferences> CreatePreferences(TenantPreferences data, IDbTransaction transaction = null);
    }
}
