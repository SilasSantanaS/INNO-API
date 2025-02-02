using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Auth;

namespace INNO.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(LoginResponseVM Result, ValidationResultVM Validation)> Authenticate(LoginRequestVM credentials);
    }
}
