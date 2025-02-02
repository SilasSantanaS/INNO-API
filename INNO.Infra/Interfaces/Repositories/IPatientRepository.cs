using INNO.Domain.Filters;
using INNO.Domain.Models;

namespace INNO.Infra.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> GetPatientById(int? id);
        Task<Patient> CreatePatient(Patient data);
        Task<int> GetTotalItems(PatientFilter filter);
        Task<bool> DeletePatient(int id, int tenantId);
        Task<Patient> UpdatePatient(int? id, Patient data);
        Task<IEnumerable<Patient>> ListPatients(PatientFilter filter);
    }
}
