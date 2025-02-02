using AutoMapper;
using INNO.Application.Interfaces.Services;
using INNO.Domain.Enums;
using INNO.Domain.Filters;
using INNO.Domain.Helpers;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Users;
using INNO.Infra.Interfaces.Repositories;

namespace INNO.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly CurrentSession _session;
        private readonly IUserRepository _userRepository;

        public UserService(
            IMapper mapper, 
            CurrentSession session, 
            IUserRepository userRepository
        )
        {
            _mapper = mapper;
            _session = session;
            _userRepository = userRepository;
        }

        public async Task<(UserResponseVM? User, ValidationResultVM Validation)> CreateUser(UserRequestVM data)
        {
            var user = _mapper.Map<User>(data);

            var validation = new ValidationResultVM();

            user = await _userRepository.CreateUser(user);

            if (user == null)
            {
                validation.Messages.Add($"Ocorreu um erro ao tentar criar o usuário.");

                return (null, validation);
            }

            return (_mapper.Map<UserResponseVM>(user), validation);
        }

        public async Task<(bool, string)> DeleteUser(int id, int? tenantId = null)
        {
            if (_session.AccessLevel != EUserProfile.ApplicationManager)
            {
                tenantId = _session.TenantId;
            }

            var wasDeleted = await _userRepository.DeleteUser(id, tenantId.Value);

            if (wasDeleted)
            {
                return (true, "Usuário removido com sucesso.");
            }

            return (false, "Usuário não encontrado.");
        }

        public async Task<UserResponseVM> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);

            return _mapper.Map<UserResponseVM>(user);
        }

        public async Task<UserListResponseVM> ListUsers(UserFilter filter)
        {
            if (_session.AccessLevel != EUserProfile.ApplicationManager)
            {
                filter.TenantId = _session.TenantId;
            }

            var users = await _userRepository.ListUsers(filter);

            return new UserListResponseVM()
            {
                Metadata = new ListMetaVM()
                {
                    Count = users.Count(),
                    Page = filter.GetPage(),
                    PageLimit = filter.GetPageLimit(),
                    TotalItems = await _userRepository.GetTotalItems(filter),
                },
                Result = _mapper.Map<IEnumerable<UserResponseVM>>(users),
            };
        }

        public async Task<UserResponseVM> UpdateUser(int id, UserPutRequestVM data)
        {
            var newUser = _mapper.Map<User>(data);
            var oldUser = await _userRepository.GetUserById(id);

            if (!string.IsNullOrWhiteSpace(data.OldPassword) && !string.IsNullOrWhiteSpace(data.NewPassword))
            {
                if (PasswordHelper.VerifyHashedPassword(oldUser.Password, data.OldPassword))
                {
                    await _userRepository.UpdateUserPassword(id, newUser.Password);
                }

                return null;
            }

            await _userRepository.UpdateUser(id, newUser);

            return await GetUserById(id);
        }
    }
}
