using TravelAgency.BLL.Common;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Internal;
using TravelAgency.BLL.Mapping;
using TravelAgency.BLL.Models.Users;
using TravelAgency.BLL.Security;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        private IRepository<User> Users => _unitOfWork.Repository<User>();

        /// <summary>Новий користувач завжди отримує роль Customer; менеджера призначає адміністратор.</summary>
        public UserModel Register(RegisterInputModel model)
        {
            string fullName = Guard.RequiredText(model.FullName, "Ім'я", BusinessRules.MaxNameLength);
            string email = EmailRules.Normalize(model.Email);
            if (!EmailRules.IsValid(email))
            {
                throw new ValidationFailedException("Некоректний email.");
            }
            if ((model.Password ?? string.Empty).Length < BusinessRules.MinPasswordLength)
            {
                throw new ValidationFailedException("Пароль має містити щонайменше " + BusinessRules.MinPasswordLength + " символів.");
            }
            if (Users.Any(u => u.Email == email))
            {
                throw new ConflictException("Користувач з таким email вже зареєстрований.");
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = _passwordHasher.Hash(model.Password),
                Role = UserRole.Customer
            };
            Users.Add(user);
            _unitOfWork.SaveChanges();
            return user.ToModel();
        }

        public UserModel Login(LoginInputModel model)
        {
            string email = EmailRules.Normalize(model.Email);
            User user = Users.FirstOrDefault(u => u.Email == email);
            // однакова відповідь для "немає email" і "неправильний пароль", щоб не підказувати, які email існують
            if (user == null || !_passwordHasher.Verify(model.Password, user.PasswordHash))
            {
                throw new InvalidCredentialsException();
            }
            return user.ToModel();
        }
    }
}
