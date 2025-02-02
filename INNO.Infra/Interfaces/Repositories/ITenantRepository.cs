using INNO.Domain.Filters;
using INNO.Domain.Models;
using System.Data;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface ITenantRepository
    {
        Task<bool> ActivateTenant(int? id);
        Task<Tenant> GetTenantById(int? id);
        Task<bool> InactivateTenant(int? id);
        Task<Tenant?> GetTenantByName(string name);
        Task<int> GetTotalItems(TenantFilter filter);
        Task<Tenant> UpdateTenant(int? id, Tenant data);
        Task<IEnumerable<Tenant>> ListTenants(TenantFilter filter);
        Task<Tenant> CreateTenant(Tenant data, IDbTransaction? transaction = null);
    }
}
