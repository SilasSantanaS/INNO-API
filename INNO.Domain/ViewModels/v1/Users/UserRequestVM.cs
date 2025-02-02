namespace INNO.Domain.ViewModels.v1.Users
{
    public class UserRequestVM
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Password { get; set; }
        public int? ProfileId { get; set; }
        public int? TenantId { get; set; }
    }
}
