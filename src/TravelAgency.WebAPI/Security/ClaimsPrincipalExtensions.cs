using System.Security.Claims;

namespace TravelAgency.WebAPI.Security
{
    internal static class ClaimsPrincipalExtensions
    {
        /// <summary>Id користувача з токена. Викликається лише в діях з [Authorize], тому claim завжди є.</summary>
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(JwtClaimNames.UserId).Value);
        }
    }
}
