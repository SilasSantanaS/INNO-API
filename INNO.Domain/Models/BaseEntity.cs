namespace INNO.Domain.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
