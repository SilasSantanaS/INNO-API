using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Users;

namespace INNO.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserResponseVM> GetUserById(int id);
        Task<UserListResponseVM> ListUsers(UserFilter filter);
        Task<(bool, string)> DeleteUser(int id, int? tenantId = null);
        Task<UserResponseVM> UpdateUser(int id, UserPutRequestVM data);
        Task<(UserResponseVM User, ValidationResultVM Validation)> CreateUser(UserRequestVM data);
    }
}
