namespace TravelAgency.WebAPI.Security
{
    /// <summary>Короткі стандартні назви claims у токені.</summary>
    internal static class JwtClaimNames
    {
        public const string UserId = "sub";
        public const string Name = "name";
        public const string Email = "email";
        public const string Role = "role";
    }
}
