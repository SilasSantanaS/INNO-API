namespace INNO.Domain.Filters
{
    public class ProfessionalFilter : ListFilter
    {
        public bool Inactive { get; set; }
        public string Search { get; set; }
    }
}
