using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Patients;

namespace INNO.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<(bool, string?)> ActivatePatient(int id);
        Task<PatientResponseVM> GetPatientById(int id);
        Task<(bool, string?)> InactivatePatient(int id);
        Task<PatientListResponseVM> ListPatients(PatientFilter filter);
        Task<PatientResponseVM> UpdatePatient(int id, PatientRequestVM data);
        Task<(PatientResponseVM Professional, ValidationResultVM Validation)> CreatePatient(PatientRequestVM data);
    }
}
