using AutoMapper;
using INNO.Application.Interfaces.Services;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Preferences;
using INNO.Infra.Interfaces.Repositories;

namespace INNO.Application.Services
{
    public class PreferencesService : IPreferencesService
    {
        private readonly IMapper _mapper;
        private readonly CurrentSession _session;
        private readonly ITenantPreferencesRepository _settingsRepository;

        public PreferencesService(
            IMapper mapper, 
            CurrentSession session, 
            ITenantPreferencesRepository settingsRepository
        )
        {
            _mapper = mapper;
            _session = session;
            _settingsRepository = settingsRepository;
        }

        public async Task<InnoSettingsResponseVM> CreatePortalSettings(InnoSettingsRequestVM data)
        {
            var settings = _mapper.Map<TenantPreferences>(data);

            settings.TenantId = _session.TenantId;
            settings = await _settingsRepository.CreatePreferences(settings);

            return _mapper.Map<InnoSettingsResponseVM>(settings);
        }

        public async Task<InnoSettingsResponseVM> GetPreferences()
        {
            var result = await _settingsRepository.GetPreferences(_session.TenantId);

            return _mapper.Map<InnoSettingsResponseVM>(result);
        }

        public Task<PreferencesOptionsResponseVM> ListPreferencesOptions()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseVM<InnoSettingsResponseVM>> UpdatePreferences(InnoSettingsRequestVM data)
        {
            var settings = await _settingsRepository.UpdatePreferences(_session.TenantId, _mapper.Map<TenantPreferences>(data));
            
            var result = _mapper.Map<InnoSettingsResponseVM>(settings);

            return new ResponseVM<InnoSettingsResponseVM>()
            {
                Result = result,
                Success = true,
                Status = 200,
            };
        }
    }
}
