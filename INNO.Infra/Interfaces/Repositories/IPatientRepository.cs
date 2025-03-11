using INNO.Domain.Filters;
using INNO.Domain.Models;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> CreatePatient(Patient data);
        Task<int> GetTotalItems(PatientFilter filter);
        Task<Patient> UpdatePatient(int? id, Patient data);
        Task<bool> ActivatePatient(int? id, int? tenantId);
        Task<bool> InactivatePatient(int? id, int? tenantId);
        Task<Patient> GetPatientById(int? id, PatientFilter filter);
        Task<IEnumerable<Patient>> ListPatients(PatientFilter filter);
    }
}
