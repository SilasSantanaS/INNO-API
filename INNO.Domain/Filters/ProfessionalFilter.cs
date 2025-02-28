namespace INNO.Domain.Filters
{
    public class ProfessionalFilter : ListFilter
    {
        public int? Id { get; set; }
        public bool? Inactive { get; set; }
        public string? Search { get; set; }
    }
}
