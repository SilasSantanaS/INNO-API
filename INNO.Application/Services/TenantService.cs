using AutoMapper;
using INNO.Application.Interfaces.Services;
using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Tenants;
using INNO.Infra.Interfaces.Repositories;

namespace INNO.Application.Services
{
    public class TenantService : ITenantService
    {
        private readonly IMapper _mapper;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantPreferencesRepository _preferencesRepository;

        public TenantService(
            IMapper mapper,
            ITenantRepository tenantRepository,
            ITenantPreferencesRepository preferencesRepository
        )
        {
            _mapper = mapper;
            _tenantRepository = tenantRepository;
            _preferencesRepository = preferencesRepository;
        }

        public async Task<bool> ActivateTenant(int id)
        {
            return await _tenantRepository.ActivateTenant(id);
        }

        public async Task<(TenantResponseVM? Result, string? Message)?> CreateTenant(TenantRequestVM data)
        {
            try
            {
                var tenant = await _tenantRepository.GetTenantByName(data.Name);

                if (tenant != null)
                {
                    return (null, "Este hostname já está em uso.");
                }

                tenant = _mapper.Map<Tenant>(data);

                tenant = await _tenantRepository.CreateTenant(tenant);

                await _preferencesRepository.CreatePreferences(new TenantPreferences()
                {
                    TokenDuration = 2,
                    InviteDuration = 20,
                    TenantId = tenant.Id,
                });

                return (_mapper.Map<TenantResponseVM>(tenant), null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TenantResponseVM> GetTenantById(int id)
        {
            var result = await _tenantRepository.GetTenantById(id);

            return _mapper.Map<TenantResponseVM>(result);
        }

        public async Task<bool> InactivateTenant(int id)
        {
            return await _tenantRepository.InactivateTenant(id);
        }

        public async Task<TenantListResponseVM> ListTenants(TenantFilter filter)
        {
            var result = await _tenantRepository.ListTenants(filter);
            var totalItems = await _tenantRepository.GetTotalItems(filter);

            return new TenantListResponseVM()
            {
                Metadata = new ListMetaVM()
                {
                    Count = result.Count(),
                    Page = filter.GetPage(),
                    TotalItems = totalItems,
                    PageLimit = filter.GetPageLimit(),
                },
                Result = _mapper.Map<IEnumerable<TenantResponseVM>>(result)
            };
        }

        public async Task<TenantResponseVM?> UpdateTenant(int id, TenantRequestVM data)
        {
            var tenant = _mapper.Map<Tenant>(data);
            
            tenant = await _tenantRepository.UpdateTenant(id, tenant);

            if(tenant == null)
            {
                return null;
            }

            return _mapper.Map<TenantResponseVM>(tenant);
        }
    }
}
