using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Preferences;

namespace INNO.Application.Interfaces.Services
{
    public interface IPreferencesService
    {
        Task<InnoSettingsResponseVM> GetPreferences();
        Task<PreferencesOptionsResponseVM> ListPreferencesOptions();
        Task<InnoSettingsResponseVM> CreatePortalSettings(InnoSettingsRequestVM data);
        Task<ResponseVM<InnoSettingsResponseVM>> UpdatePreferences(InnoSettingsRequestVM data);
    }
}
