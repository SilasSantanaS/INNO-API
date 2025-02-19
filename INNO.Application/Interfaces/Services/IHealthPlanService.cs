using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.HealthPlans;

namespace INNO.Application.Interfaces.Services
{
    public interface IHealthPlanService
    {
        Task<HealthPlanResponseVM> GetHealthPlanById(int id);
        Task<bool> DeleteHealthPlan(int id, int? tenantId = null);
        Task<HealthPlanListResponseVM> ListHealthPlans(HealthPlanFilter filter);
        Task<HealthPlanResponseVM> UpdateHealthPlan(int id, HealthPlanRequestVM data);
        Task<(HealthPlanResponseVM HealthPlan, ValidationResultVM Validation)> CreateHealthPlan(HealthPlanRequestVM data);
    }
}
