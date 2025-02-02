using AutoMapper;
using INNO.Application.Interfaces.Services;
using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Patients;
using INNO.Infra.Interfaces.Repositories;

namespace INNO.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IMapper _mapper;
        private readonly CurrentSession _session;
        private readonly IPatientRepository _patientRepository;

        public PatientService(
            IMapper mapper, 
            CurrentSession session, 
            IPatientRepository patientRepository
        )
        {
            _mapper = mapper;
            _session = session;
            _patientRepository = patientRepository;
        }

        public async Task<(PatientResponseVM? Professional, ValidationResultVM? Validation)> CreatePatient(PatientRequestVM data)
        {
            var patient = _mapper.Map<Patient>(data);

            patient = await _patientRepository.CreatePatient(patient);

            return (_mapper.Map<PatientResponseVM>(patient), null);
        }

        public Task<(bool, string)> DeletePatient(int id, int? tenantId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientResponseVM> GetPatientById(int id)
        {
            var result = await _patientRepository.GetPatientById(id);

            return _mapper.Map<PatientResponseVM>(result);
        }

        public async Task<PatientListResponseVM> ListPatients(PatientFilter filter)
        {
            var result = await _patientRepository.ListPatients(filter);
            var totalItems = await _patientRepository.GetTotalItems(filter);

            return new PatientListResponseVM()
            {
                Metadata = new ListMetaVM()
                {
                    Count = result.Count(),
                    Page = filter.GetPage(),
                    TotalItems = totalItems,
                    PageLimit = filter.GetPageLimit(),
                },
                Result = _mapper.Map<IEnumerable<PatientResponseVM>>(result)
            };
        }

        public Task<PatientResponseVM> UpdatePatient(int id, PatientRequestVM data)
        {
            throw new NotImplementedException();
        }
    }
}
