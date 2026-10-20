using TravelAgency.BLL.Exceptions;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Internal
{
    internal static class UserGuard
    {
        /// <summary>Повертає користувача або кидає AccessDenied, якщо такого немає.</summary>
        public static User GetRegistered(IUnitOfWork unitOfWork, int userId)
        {
            User user = unitOfWork.Repository<User>().GetById(userId);
            if (user == null)
            {
                throw new AccessDeniedException("Дія доступна тільки зареєстрованим користувачам.");
            }
            return user;
        }

        public static bool IsStaff(User user)
        {
            return user.Role == UserRole.Manager || user.Role == UserRole.Admin;
        }
    }
}
