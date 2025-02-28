namespace INNO.Domain.ViewModels.v1.Addresses
{
    public class AddressResponseVM
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
    }
}
