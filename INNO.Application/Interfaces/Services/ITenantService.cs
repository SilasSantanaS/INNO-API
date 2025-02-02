using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1.Tenants;

namespace INNO.Application.Interfaces.Services
{
    public interface ITenantService
    {
        Task<bool> ActivateTenant(int id);
        Task<bool> InactivateTenant(int id);
        Task<TenantResponseVM> GetTenantById(int id);
        Task<TenantListResponseVM> ListTenants(TenantFilter filter);
        Task<TenantResponseVM?> UpdateTenant(int id, TenantRequestVM data);
        Task<(TenantResponseVM Result, string Message)?> CreateTenant(TenantRequestVM data);
    }
}
