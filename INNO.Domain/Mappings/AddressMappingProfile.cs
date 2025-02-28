using AutoMapper;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1.Addresses;

namespace INNO.Domain.Mappings
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            CreateMap<AddressRequestVM, Address>();
            CreateMap<Address, AddressResponseVM>();
        }
    }
}
