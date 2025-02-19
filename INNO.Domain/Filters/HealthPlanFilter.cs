namespace INNO.Domain.Filters
{
    public class HealthPlanFilter : ListFilter
    {
        public bool? Inactive { get; set; }
        public string? Search { get; set; }
    }
}
