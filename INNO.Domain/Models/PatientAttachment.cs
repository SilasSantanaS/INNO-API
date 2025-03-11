namespace INNO.Domain.Models
{
    public class PatientAttachment : Attachment
    {
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}
