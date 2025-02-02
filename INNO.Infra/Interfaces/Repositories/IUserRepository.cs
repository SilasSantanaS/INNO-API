using INNO.Domain.Filters;
using INNO.Domain.Models;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int? id);
        Task<User> CreateUser(User data);
        Task<User> GetUserByEmail(string email);
        Task<User> UpdateUser(int? id, User data);
        Task<int> GetTotalItems(UserFilter filter);
        Task<bool> DeleteUser(int id, int tenantId);
        Task<IEnumerable<User>> ListUsers(UserFilter filter);
        Task<bool> UpdateUserPassword(int? id, string password);

    }
}
