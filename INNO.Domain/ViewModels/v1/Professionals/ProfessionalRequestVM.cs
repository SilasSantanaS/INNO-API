using INNO.Domain.ViewModels.v1.Addresses;
using INNO.Domain.ViewModels.v1.Contacts;

namespace INNO.Domain.ViewModels.v1.Professionals
{
    public class ProfessionalRequestVM
    {
        public string? Name { get; set; }
        public string? Document { get; set; }
        public ContactRequestVM? Contact { get; set; }
        public AddressRequestVM? Address { get; set; }
    }
}
