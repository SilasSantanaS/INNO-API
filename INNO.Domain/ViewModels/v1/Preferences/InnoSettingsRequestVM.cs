namespace INNO.Domain.ViewModels.v1.Preferences
{
    public class InnoSettingsRequestVM
    {
        public int? ThemeId { get; set; }
        public int? TokenDuration { get; set; }
        public int? InviteDuration { get; set; }
        public string? TelWhatsapp { get; set; }
    }
}
