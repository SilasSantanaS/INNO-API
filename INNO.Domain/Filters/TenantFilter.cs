namespace INNO.Domain.Filters
{
    public class TenantFilter : ListFilter
    {
        public string? Search { get; set; }
        public bool Inactive { get; set; }
        public DateTime? InactivatedAfter { get; set; }
    }
}
