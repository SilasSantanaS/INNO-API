using INNO.Domain.Filters;
using INNO.Domain.Models;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface IHealthPlanRepository
    {
        Task<int> GetTotalItems(HealthPlanFilter filter);
        Task<bool> DeleteHealthPlan(int id, int tenantId);
        Task<HealthPlan> CreateHealthPlan(HealthPlan data);
        Task<HealthPlan> GetHealthPlanById(int? id, int? tenantId);
        Task<HealthPlan> UpdateHealthPlan(int? id, HealthPlan data);
        Task<IEnumerable<HealthPlan>> ListHealthPlans(HealthPlanFilter filter);
    }
}
