namespace TravelAgency.WebAPI.Security
{
    /// <summary>Налаштування JWT із розділу "Jwt" файлу appsettings.json.</summary>
    internal class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key { get; set; }

        public int LifetimeMinutes { get; set; }
    }
}
