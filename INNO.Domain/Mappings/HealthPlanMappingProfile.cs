using AutoMapper;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1.HealthPlans;

namespace INNO.Domain.Mappings
{
    public class HealthPlanMappingProfile : Profile
    {
        public HealthPlanMappingProfile()
        {
            CreateMap<HealthPlanRequestVM, HealthPlan>();
            CreateMap<HealthPlan, HealthPlanResponseVM>();
        }
    }
}
