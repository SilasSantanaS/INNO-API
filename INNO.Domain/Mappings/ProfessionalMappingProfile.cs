using AutoMapper;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1.Professionals;

namespace INNO.Domain.Mappings
{
    public class ProfessionalMappingProfile : Profile
    {
        public ProfessionalMappingProfile()
        {
            CreateMap<ProfessionalRequestVM, Professional>();
            CreateMap<Professional, ProfessionalResponseVM>();
        }
    }
}
