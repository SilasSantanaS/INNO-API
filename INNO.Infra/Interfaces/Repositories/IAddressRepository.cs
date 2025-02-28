using INNO.Domain.Models;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> CreateAddress(Address data);
        Task<bool> DeleteAddress(int? id, int? tenantId);
        Task<Address> UpdateAddress(int? id, Address data);
        Task<Address> GetAddressById(int? id, int? tenantId);
    }
}
