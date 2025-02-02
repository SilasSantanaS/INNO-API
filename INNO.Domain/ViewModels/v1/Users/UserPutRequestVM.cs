namespace INNO.Domain.ViewModels.v1.Users
{
    public class UserPutRequestVM
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public int? ProfileId { get; set; }
        public int? TenantId { get; set; }
    }
}
