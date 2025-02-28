namespace INNO.Domain.Models
{
    public class Contact : BaseEntity
    {
        public string? Obs { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
