using AutoMapper;
using INNO.Application.Interfaces.Services;
using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Professionals;
using INNO.Infra.Interfaces.Repositories;

namespace INNO.Application.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly IMapper _mapper;
        private readonly CurrentSession _session;
        private readonly IProfessionalRepository _professionalRepository;

        public ProfessionalService(
            IMapper mapper, 
            CurrentSession session, 
            IProfessionalRepository professionalRepository
        )
        {
            _mapper = mapper;
            _session = session;
            _professionalRepository = professionalRepository;
        }

        public async Task<(ProfessionalResponseVM? Professional, ValidationResultVM? Validation)> CreateProfessional(ProfessionalRequestVM data)
        {
            var professional = _mapper.Map<Professional>(data);

            professional = await _professionalRepository.CreateProfessional(professional);

            return (_mapper.Map<ProfessionalResponseVM>(professional), null);
        }

        public Task<(bool, string)> DeleteProfessional(int id, int? tenantId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ProfessionalResponseVM> GetProfessionalById(int id)
        {
            var result = await _professionalRepository.GetProfessionalById(id);

            return _mapper.Map<ProfessionalResponseVM>(result);
        }

        public async Task<ProfessionalListResponseVM> ListProfessionals(ProfessionalFilter filter)
        {
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
            throw new NotImplementedException();
        }
    }
}
