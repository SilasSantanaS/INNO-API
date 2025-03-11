namespace INNO.Domain.Models
{
    public class Patient : BaseEntity
    {
        public string? Name { get; set; }
        public string? MotherName { get; set; }
        public string? FatherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? HealthPlanId { get; set; }
        public HealthPlan? HealthPlan { get; set; }
        public int? ContactId { get; set; }
        public Contact? Contact { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public DateTime? InactivatedAt { get; set; }
    }
}
