using INNO.Domain.ViewModels.v1.Users;

namespace INNO.Domain.ViewModels.v1.Auth
{
    public class LoginResponseVM
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public UserResponseVM? User { get; set; }
    }
}
