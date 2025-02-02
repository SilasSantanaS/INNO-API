namespace INNO.Domain.Filters
{
    public class UserFilter : ListFilter
    {
        public bool? Inactive { get; set; }
        public string? Search { get; set; }
        public IEnumerable<int>? ProfileId { get; set; }
    }
}
