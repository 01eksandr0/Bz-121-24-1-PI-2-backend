namespace TravelAgency.BLL.Internal
{
    internal static class EmailNormalizer
    {
        public static string Normalize(string email)
        {
            return (email ?? string.Empty).Trim().ToLowerInvariant();
        }

        /// <summary>Проста перевірка: є @, частина до нього і домен з крапкою.</summary>
        public static bool IsValid(string email)
        {
            int at = email.IndexOf('@');
            return at > 0 && at == email.LastIndexOf('@') && email.IndexOf('.', at) > at + 1 && !email.EndsWith(".");
        }
    }
}
