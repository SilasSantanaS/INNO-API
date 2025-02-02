namespace INNO.Domain.ViewModels.v1.Tenants
{
    public class TenantResponseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CorporateName { get; set; }
        public int PricingTierId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? InactivatedAt { get; set; }
    }
}
