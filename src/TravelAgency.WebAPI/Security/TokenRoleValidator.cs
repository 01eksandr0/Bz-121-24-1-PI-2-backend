using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Models.Users;

namespace TravelAgency.WebAPI.Security
{
    /// <summary>
    /// Роль записується в токен при вході. Якщо адміністратор змінив роль, старий токен не повинен
    /// давати старих прав, тому на кожному запиті роль з токена звіряється з поточною роллю в БД.
    /// </summary>
    internal static class TokenRoleValidator
    {
        private const string RoleChangedMessage = "Роль користувача змінилась - потрібно увійти знову.";

        public static Task ValidateAsync(TokenValidatedContext context)
        {
            var users = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            string tokenRole = context.Principal.FindFirst(JwtClaimNames.Role)?.Value;
            try
            {
                UserModel user = users.GetById(context.Principal.GetUserId());
                if (user.Role.ToString() != tokenRole)
                {
                    context.Fail(RoleChangedMessage);
                }
            }
            catch (EntityNotFoundException)
            {
                context.Fail(RoleChangedMessage);
            }
            return Task.CompletedTask;
        }
    }
}
