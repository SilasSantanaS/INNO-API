using AutoMapper;
using INNO.Application.Helpers;
using INNO.Application.Interfaces.Services;
using INNO.Domain.Helpers;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Auth;
using INNO.Domain.ViewModels.v1.Users;
using INNO.Infra.Interfaces.Repositories;

namespace INNO.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthService(
            IMapper mapper,
            IUserRepository userRepository
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<(LoginResponseVM? Result, ValidationResultVM? Validation)> Authenticate(LoginRequestVM credentials)
        {
            var user = await _userRepository.GetUserByEmail(credentials.Email);

            if(user == null)
            {
                return (null, new ValidationResultVM() 
                {
                    Messages = ["E-mail e/ou senha inválidos."]
                });
            }

            var valid = PasswordHelper.VerifyHashedPassword(user.Password, credentials.Password);

            if(!valid)
            {
                return (null, new ValidationResultVM()
                {
                    Messages = ["E-mail e/ou senha inválidos."]
                });
            }

            var token = JwtHelper.GenerateToken(user);
            var refreshToken = JwtHelper.GenerateRefreshToken();

            var result = new LoginResponseVM() 
            {
                Token = token,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserResponseVM>(user)
            };

            return (result, null);
        }
    }
}
