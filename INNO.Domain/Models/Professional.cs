namespace INNO.Domain.Models
{
    public class Professional : BaseEntity
    {
        public string? Name { get; set; }
        public int? UserId { get; set; }
        public int? ContactId { get; set; }
        public Contact? Contact { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public DateTime? InactivatedAt { get; set; }
    }
}
