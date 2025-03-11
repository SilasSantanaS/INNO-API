using INNO.Domain.Enums;
using INNO.Domain.ViewModels.v1.Addresses;
using INNO.Domain.ViewModels.v1.Contacts;
using INNO.Domain.ViewModels.v1.Users;

namespace INNO.Domain.ViewModels.v1.Patients
{
    public class PatientResponseVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? MotherName { get; set; }
        public string? FatherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? NationalId { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public UserResponseVM? User { get; set; }
        public AddressResponseVM? Address { get; set; }
        public ContactResponseVM? Contact { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? InactivatedAt { get; set; }
    }
}
