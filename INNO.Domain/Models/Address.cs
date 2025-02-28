namespace INNO.Domain.Models
{
    public class Address : BaseEntity
    {
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? ZipCode { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
    }
}
