using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Professionals;

namespace INNO.Application.Interfaces.Services
{
    public interface IProfessionalService
    {
        Task<ProfessionalResponseVM> GetProfessionalById(int id);
        Task<(bool, string)> DeleteProfessional(int id, int? tenantId = null);
        Task<ProfessionalListResponseVM> ListProfessionals(ProfessionalFilter filter);
        Task<ProfessionalResponseVM> UpdateProfessional(int id, ProfessionalRequestVM data);
        Task<(ProfessionalResponseVM Professional, ValidationResultVM Validation)> CreateProfessional(ProfessionalRequestVM data);
    }
}
