using INNO.Domain.Filters;
using INNO.Domain.Models;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface IProfessionalRepository
    {
        Task<int> GetTotalItems(ProfessionalFilter filter);
        Task<bool> DeleteProfessional(int id, int tenantId);
        Task<bool> ActivateProfessinal(int? id, int? tenantId);
        Task<Professional> CreateProfessional(Professional data);
        Task<bool> InactivateProfessional(int? id, int? tenantId);
        Task<Professional> UpdateProfessional(int? id, Professional data);
        Task<Professional> GetProfessionalById(int? id, ProfessionalFilter filter);
        Task<IEnumerable<Professional>> ListProfessionals(ProfessionalFilter filter);
    }
}
