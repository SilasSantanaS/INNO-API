using INNO.Domain.Enums;

namespace INNO.Domain.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Document { get; set; }
        public EUserProfile ProfileId { get; set; }
        public DateTime? InactivatedAt { get; set; }
    }
}
