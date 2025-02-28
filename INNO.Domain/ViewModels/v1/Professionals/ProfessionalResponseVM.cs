using INNO.Domain.Enums;
using INNO.Domain.ViewModels.v1.Addresses;
using INNO.Domain.ViewModels.v1.Contacts;
using INNO.Domain.ViewModels.v1.Users;

namespace INNO.Domain.ViewModels.v1.Professionals
{
    public class ProfessionalResponseVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public EUserProfile RoleId { get; set; }
        public string? NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? ProfessionalLicense { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public DateTime DateOfHire { get; set; }
        public DateTime DateOfPayment { get; set; }
        public string? PaymentInformation { get; set; }
        public UserResponseVM? User { get; set; }
        public AddressResponseVM? Address { get; set; }
        public ContactResponseVM? Contact { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? InactivatedAt { get; set; }
    }
}
