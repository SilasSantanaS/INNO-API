using INNO.Domain.Enums;

namespace INNO.Domain.Models
{
    public class CurrentSession
    {
        public int UserId { get; set; }
        public int? TenantId { get; set; }
        public string Email { get; set; }
        public EUserProfile AccessLevel { get; set; }
    }
}
