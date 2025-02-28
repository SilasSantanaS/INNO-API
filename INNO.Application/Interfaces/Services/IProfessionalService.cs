using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Professionals;

namespace INNO.Application.Interfaces.Services
{
    public interface IProfessionalService
    {
        Task<(bool, string?)> ActivateProfessional(int id);
        Task<(bool, string?)> InactivateProfessional(int id);
        Task<ProfessionalResponseVM> GetProfessionalById(int id);
        Task<ProfessionalListResponseVM> ListProfessionals(ProfessionalFilter filter);
        Task<ProfessionalResponseVM> UpdateProfessional(int id, ProfessionalRequestVM data);
        Task<(ProfessionalResponseVM Professional, ValidationResultVM Validation)> CreateProfessional(ProfessionalRequestVM data);
    }
}
