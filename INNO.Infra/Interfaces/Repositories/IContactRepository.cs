using INNO.Domain.Models;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface IContactRepository
    {
        Task<Contact> CreateContact(Contact data);
        Task<bool> DeleteContact(int? id, int? tenantId);
        Task<Contact> UpdateContact(int? id, Contact data);
        Task<Contact> GetContactById(int? id, int? tenantId);
    }
}
