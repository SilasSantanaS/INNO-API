namespace INNO.Domain.Filters
{
    public class PatientFilter : ListFilter
    {
        public int? Id { get; set; }
        public bool? Inactive { get; set; }
        public string? Search { get; set; }
    }
}
