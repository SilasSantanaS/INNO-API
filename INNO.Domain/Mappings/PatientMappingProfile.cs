using AutoMapper;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1.Patients;

namespace INNO.Domain.Mappings
{
    public class PatientMappingProfile : Profile
    {
        public PatientMappingProfile()
        {
            CreateMap<PatientRequestVM, Patient>();
            CreateMap<Patient, PatientResponseVM>();
        }
    }
}
