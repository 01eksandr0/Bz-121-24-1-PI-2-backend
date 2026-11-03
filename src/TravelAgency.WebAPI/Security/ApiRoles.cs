using TravelAgency.BLL.Models;

namespace TravelAgency.WebAPI.Security
{
    /// <summary>
    /// Ролі для [Authorize(Roles = ...)] - це назви переліку UserRole, тому розійтися вони не можуть.
    /// Доступ до дії за роллю перевіряє PL (роль у токені звіряється з БД на кожному запиті - TokenRoleValidator),
    /// а правила щодо конкретного об'єкта (чиє бронювання, власна роль) - BLL.
    /// </summary>
    internal static class ApiRoles
    {
        public const string Manager = nameof(UserRole.Manager);
        public const string Admin = nameof(UserRole.Admin);

        /// <summary>Персонал агенції: менеджер і адміністратор.</summary>
        public const string Staff = Manager + "," + Admin;
    }
}
