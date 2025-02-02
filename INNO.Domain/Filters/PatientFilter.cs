namespace INNO.Domain.Filters
{
    public class PatientFilter : ListFilter
    {
        public bool Inactive { get; set; }
        public string Search { get; set; }
    }
}
