using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Patients;

namespace INNO.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<PatientResponseVM> GetPatientById(int id);
        Task<PatientListResponseVM> ListPatients(PatientFilter filter);
        Task<PatientResponseVM> UpdatePatient(int id, PatientRequestVM data);
        Task<(bool, string)> DeletePatient(int id, int? tenantId = null);
        Task<(PatientResponseVM Professional, ValidationResultVM Validation)> CreatePatient(PatientRequestVM data);
    }
}
