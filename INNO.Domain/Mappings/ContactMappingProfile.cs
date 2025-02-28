using AutoMapper;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1.Contacts;

namespace INNO.Domain.Mappings
{
    public class ContactMappingProfile : Profile
    {
        public ContactMappingProfile()
        {
            CreateMap<ContactRequestVM, Contact>();
            CreateMap<Contact, ContactResponseVM>();
        }
    }
}
