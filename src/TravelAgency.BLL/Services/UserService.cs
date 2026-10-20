using System.Collections.Generic;
using System.Linq;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Internal;
using TravelAgency.BLL.Mapping;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Users;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Services
{
    public class UserService : IUserService
    {
        private const string EntityName = "Користувача";

        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private IRepository<User> Users => _unitOfWork.Repository<User>();

        public IReadOnlyList<UserModel> GetAll()
        {
            return Users.GetAll().OrderBy(u => u.FullName).Select(u => u.ToModel()).ToList();
        }

        public UserModel GetById(int id)
        {
            return Users.GetRequired(id, EntityName).ToModel();
        }

        /// <summary>Адміністратор змінює роль іншого користувача (власну - ні, щоб не залишити систему без адміністратора).</summary>
        public UserModel ChangeRole(int actingUserId, int userId, Role role)
        {
            if (actingUserId == userId)
            {
                throw new AccessDeniedException("Не можна змінити власну роль.");
            }
            User user = Users.GetRequired(userId, EntityName);
            user.Role = role.ToEntity();
            Users.Update(user);
            _unitOfWork.SaveChanges();
            return user.ToModel();
        }
    }
}
