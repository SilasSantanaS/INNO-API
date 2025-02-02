using AutoMapper;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1.Tenants;

namespace INNO.Domain.Mappings
{
    public class TenantMappingProfile : Profile
    {
        public TenantMappingProfile()
        {
            CreateMap<TenantRequestVM, Tenant>();
            CreateMap<Tenant, TenantResponseVM>();
        }
    }
}
