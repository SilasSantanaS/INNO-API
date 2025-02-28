using AutoMapper;
using INNO.Application.Interfaces.Services;
using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.Utils;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Professionals;
using INNO.Infra.Interfaces.Repositories;

namespace INNO.Application.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly IMapper _mapper;
        private readonly CurrentSession _session;
        private readonly IContactRepository _contactRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IProfessionalRepository _professionalRepository;

        public ProfessionalService(
            IMapper mapper, 
            CurrentSession session,
            IContactRepository contactRepository,
            IAddressRepository addressRepository,
            IProfessionalRepository professionalRepository
        )
        {
            _mapper = mapper;
            _session = session;
            _contactRepository = contactRepository;
            _addressRepository = addressRepository;
            _professionalRepository = professionalRepository;
        }

        public async Task<(ProfessionalResponseVM? Professional, ValidationResultVM? Validation)> CreateProfessional(ProfessionalRequestVM data)
        {
            var valid = Validate(data);

            if(!valid.IsValid)
            {
                return (null, valid);
            }

            var professional = _mapper.Map<Professional>(data);

            professional.TenantId = _session.TenantId;

            professional.AddressId = await CreateAddress(professional);
            professional.ContactId = await CreateContact(professional);

            professional = await _professionalRepository.CreateProfessional(professional);

            var result = await GetProfessionalById(professional.Id);

            return (result, null);
        }

        public async Task<(bool, string?)> ActivateProfessional(int id)
        {
            var result = await _professionalRepository.ActivateProfessinal(id, _session.TenantId);

            return (result, null);
        }

        public async Task<(bool, string?)> InactivateProfessional(int id)
        {
            var result = await _professionalRepository.InactivateProfessional(id, _session.TenantId);

            return (result, null);
        }

        public async Task<ProfessionalResponseVM> GetProfessionalById(int id)
        {
            var filter = new ProfessionalFilter()
            {
                TenantId = _session.TenantId
            };

            var result = await _professionalRepository.GetProfessionalById(id, filter);

            return _mapper.Map<ProfessionalResponseVM>(result);
        }

        public async Task<ProfessionalListResponseVM> ListProfessionals(ProfessionalFilter filter)
        {
            filter.TenantId = _session.TenantId;

            var result = await _professionalRepository.ListProfessionals(filter);
            var totalItems = await _professionalRepository.GetTotalItems(filter);

            return new ProfessionalListResponseVM()
            {
                Metadata = new ListMetaVM()
                {
                    Count = result.Count(),
                    Page = filter.GetPage(),
                    TotalItems = totalItems,
                    PageLimit = filter.GetPageLimit(),
                },
                Result = _mapper.Map<IEnumerable<ProfessionalResponseVM>>(result)
            };
        }

        public async Task<ProfessionalResponseVM> UpdateProfessional(int id, ProfessionalRequestVM data)
        {
            var professional = _mapper.Map<Professional>(data);

            professional.TenantId = _session.TenantId;

            var result = await _professionalRepository.UpdateProfessional(id, professional);

            await UpdateAddress(result.AddressId, professional);
            await UpdateContact(result.ContactId, professional);

            return await GetProfessionalById(id);
        }

        private async Task<int?> CreateAddress(Professional professional)
        {
            if (professional?.Address == null)
            {
                return null;
            }

            professional.Address.TenantId = _session.TenantId;

            var result = await _addressRepository.CreateAddress(professional.Address);

            return result?.Id;
        }

        private async Task<int?> UpdateAddress(int? addressId, Professional professional)
        {
            if (addressId == null || professional.Address == null)
            {
                return null;
            }

            professional.Address.TenantId = _session.TenantId;

            var result = await _addressRepository.UpdateAddress(addressId, professional.Address);

            return result?.Id;
        }

        private async Task<int?> CreateContact(Professional professional)
        {
            if (professional?.Contact == null)
            {
                return null;
            }

            professional.Contact.TenantId = _session.TenantId;

            var result = await _contactRepository.CreateContact(professional.Contact);

            return result?.Id;
        }

        private async Task<int?> UpdateContact(int? contactId, Professional professional)
        {
            if (contactId == null || professional.Contact == null)
            {
                return null;
            }

            professional.Contact.TenantId = _session.TenantId;

            var result = await _contactRepository.UpdateContact(contactId, professional.Contact);

            return result?.Id;
        }

        private ValidationResultVM Validate(ProfessionalRequestVM professional)
        {
            bool valid;
            var validation = new ValidationResultVM();

            valid = StringUtils.ValidateDocument(professional?.Document);

            if (!valid)
            {
                validation.Messages.Add("CPF inválido.");
            }

            valid = StringUtils.ValidateZipCode(professional?.Address?.ZipCode);

            if(!valid)
            {
                validation.Messages.Add("CEP inválido.");
            }            

            return validation;
        }       
    }
}
