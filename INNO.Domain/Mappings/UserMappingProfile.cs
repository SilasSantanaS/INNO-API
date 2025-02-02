using AutoMapper;
using INNO.Domain.Helpers;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1.Users;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace INNO.Domain.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponseVM>()
                .ForMember(a => a.ProfileId, b => b.MapFrom(c => (int)c.ProfileId))
                .ForMember(a => a.Profile, b => b.MapFrom(c => c.ProfileId.ToString()));

            CreateMap<UserRequestVM, User>()
                .ForMember(a => a.Password, b => b.MapFrom(c => PasswordHelper.GetHash(c.Password, KeyDerivationPrf.HMACSHA512)));

            CreateMap<UserPutRequestVM, User>()
                .ForMember(a => a.Password, b => b.MapFrom(c => PasswordHelper.GetHash(c.NewPassword, KeyDerivationPrf.HMACSHA512)));
        }
    }
}
