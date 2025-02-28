namespace INNO.Domain.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ProfessionalId { get; set; }
        public int UserId { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public string? Obs { get; set; }
    }
}
