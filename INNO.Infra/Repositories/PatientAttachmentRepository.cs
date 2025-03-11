using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace INNO.Infra.Repositories
{
    public class PatientAttachmentRepository : Repository, IPatientAttachmentRepository
    {
        public PatientAttachmentRepository(
            IDbConnectionFactory dbConnectionFactory, 
            CurrentSession session, 
            IOptionsSnapshot<DbSettings> settings
        ) : base(dbConnectionFactory, session, settings)
        {
        }
    }
}
