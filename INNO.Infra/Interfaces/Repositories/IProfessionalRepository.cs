using INNO.Domain.Filters;
using INNO.Domain.Models;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface IProfessionalRepository
    {
        Task<Professional> GetProfessionalById(int? id);
        Task<int> GetTotalItems(ProfessionalFilter filter);
        Task<bool> DeleteProfessional(int id, int tenantId);
        Task<Professional> CreateProfessional(Professional data);
        Task<Professional> UpdateProfessional(int? id, Professional data);
        Task<IEnumerable<Professional>> ListProfessionals(ProfessionalFilter filter);
    }
}
