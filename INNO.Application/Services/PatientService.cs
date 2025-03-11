using AutoMapper;
using INNO.Application.Interfaces.Services;
using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.Utils;
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
        private readonly IContactRepository _contactRepository;
        private readonly IAddressRepository _addressRepository;

        public PatientService(
            IMapper mapper, 
            CurrentSession session, 
            IPatientRepository patientRepository,
            IContactRepository contactRepository,
            IAddressRepository addressRepository
        )
        {
            _mapper = mapper;
            _session = session;
            _patientRepository = patientRepository;
            _contactRepository = contactRepository;
            _addressRepository = addressRepository;
        }

        public async Task<(bool, string?)> ActivatePatient(int id)
        {
            var result = await _patientRepository.ActivatePatient(id, _session.TenantId);

            return (result, null);
        }

        public async Task<(PatientResponseVM? Professional, ValidationResultVM? Validation)> CreatePatient(PatientRequestVM data)
        {
            var valid = Validate(data);

            if (!valid.IsValid)
            {
                return (null, valid);
            }

            var patient = _mapper.Map<Patient>(data);

            patient.TenantId = _session.TenantId;

            patient.AddressId = await CreateAddress(patient);
            patient.ContactId = await CreateContact(patient);

            patient = await _patientRepository.CreatePatient(patient);

            var result = await GetPatientById(patient.Id);

            return (result, null);
        }

        public async Task<PatientResponseVM> GetPatientById(int id)
        {
            var filter = new PatientFilter()
            {
                TenantId = _session.TenantId
            };

            var result = await _patientRepository.GetPatientById(id, filter);

            return _mapper.Map<PatientResponseVM>(result);
        }

        public async Task<(bool, string?)> InactivatePatient(int id)
        {
            var result = await _patientRepository.InactivatePatient(id, _session.TenantId);

            return (result, null);
        }

        public async Task<PatientListResponseVM> ListPatients(PatientFilter filter)
        {
            filter.TenantId = _session.TenantId;

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

        public async Task<PatientResponseVM> UpdatePatient(int id, PatientRequestVM data)
        {
            var patient = _mapper.Map<Patient>(data);

            patient.TenantId = _session.TenantId;

            var result = await _patientRepository.UpdatePatient(id, patient);

            await UpdateAddress(result.AddressId, patient);
            await UpdateContact(result.ContactId, patient);

            return await GetPatientById(id);
        }

        private async Task<int?> CreateAddress(Patient patient)
        {
            if (patient?.Address == null)
            {
                return null;
            }

            patient.Address.TenantId = _session.TenantId;

            var result = await _addressRepository.CreateAddress(patient.Address);

            return result?.Id;
        }

        private async Task<int?> UpdateAddress(int? addressId, Patient patient)
        {
            if (addressId == null || patient.Address == null)
            {
                return null;
            }

            patient.Address.TenantId = _session.TenantId;

            var result = await _addressRepository.UpdateAddress(addressId, patient.Address);

            return result?.Id;
        }

        private async Task<int?> CreateContact(Patient patient)
        {
            if (patient?.Contact == null)
            {
                return null;
            }

            patient.Contact.TenantId = _session.TenantId;

            var result = await _contactRepository.CreateContact(patient.Contact);

            return result?.Id;
        }

        private async Task<int?> UpdateContact(int? contactId, Patient patient)
        {
            if (contactId == null || patient.Contact == null)
            {
                return null;
            }

            patient.Contact.TenantId = _session.TenantId;

            var result = await _contactRepository.UpdateContact(contactId, patient.Contact);

            return result?.Id;
        }

        private ValidationResultVM Validate(PatientRequestVM patient)
        {
            bool valid;
            var validation = new ValidationResultVM();

            valid = StringUtils.ValidateDocument(patient?.Document);

            if (!valid)
            {
                validation.Messages.Add("CPF inválido.");
            }

            valid = StringUtils.ValidateZipCode(patient?.Address?.ZipCode);

            if (!valid)
            {
                validation.Messages.Add("CEP inválido.");
            }

            return validation;
        }
    }
}
