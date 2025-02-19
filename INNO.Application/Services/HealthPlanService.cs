using AutoMapper;
using INNO.Application.Interfaces.Services;
using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.HealthPlans;
using INNO.Infra.Interfaces.Repositories;

namespace INNO.Application.Services
{
    public class HealthPlanService : IHealthPlanService
    {
        private readonly IMapper _mapper;
        private readonly CurrentSession _session;
        private readonly IHealthPlanRepository _healthPlanRepository;

        public HealthPlanService(
            IMapper mapper, 
            CurrentSession session, 
            IHealthPlanRepository healthPlanRepository
        )
        {
            _mapper = mapper;
            _session = session;
            _healthPlanRepository = healthPlanRepository;
        }

        public async Task<(HealthPlanResponseVM HealthPlan, ValidationResultVM? Validation)> CreateHealthPlan(HealthPlanRequestVM data)
        {
            var healthPlan = _mapper.Map<HealthPlan>(data);

            healthPlan.TenantId = _session.TenantId;

            healthPlan = await _healthPlanRepository.CreateHealthPlan(healthPlan);

            return (_mapper.Map<HealthPlanResponseVM>(healthPlan), null);
        }

        public async Task<bool> DeleteHealthPlan(int id, int? tenantId = null)
        {
            return await _healthPlanRepository.DeleteHealthPlan(id, _session.TenantId ?? 0);
        }

        public async Task<HealthPlanResponseVM> GetHealthPlanById(int id)
        {
            var healthPlan = await _healthPlanRepository.GetHealthPlanById(id, _session.TenantId);

            return _mapper.Map<HealthPlanResponseVM>(healthPlan);
        }

        public async Task<HealthPlanListResponseVM> ListHealthPlans(HealthPlanFilter filter)
        {
            filter.TenantId = _session.TenantId;

            var healthPlans = await _healthPlanRepository.ListHealthPlans(filter);

            return new HealthPlanListResponseVM()
            {
                Metadata = new ListMetaVM()
                {
                    Page = filter.GetPage(),
                    Count = healthPlans.Count(),
                    PageLimit = filter.GetPageLimit(),
                    TotalItems = await _healthPlanRepository.GetTotalItems(filter),
                },
                Result = _mapper.Map<IEnumerable<HealthPlanResponseVM>>(healthPlans),
            };
        }

        public async Task<HealthPlanResponseVM> UpdateHealthPlan(int id, HealthPlanRequestVM data)
        {
            var healthPlan = _mapper.Map<HealthPlan>(data);

            healthPlan.TenantId = _session.TenantId;
         
            await _healthPlanRepository.UpdateHealthPlan(id, healthPlan);

            return await GetHealthPlanById(id);
        }
    }
}
