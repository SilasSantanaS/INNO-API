﻿namespace INNO.Domain.Settings
{
    public class TenantPreferences
    {
        public int? TenantId { get; set; }
        public int? TokenDuration { get; set; }
        public int? InviteDuration { get; set; }
    }
}
