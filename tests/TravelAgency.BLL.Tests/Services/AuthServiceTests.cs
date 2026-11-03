using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Users;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _service = new AuthService(_uow, new FakePasswordHasher());
        }

        [Fact]
        public void Register_NewUser_GetsCustomerRoleAndHashedPassword()
        {
            // Arrange
            var model = new RegisterInputModel { FullName = "Іван", Email = " Ivan@Mail.com ", Password = "Password1" };

            // Act
            var user = _service.Register(model);

            // Assert
            Assert.Equal(UserRole.Customer, user.Role);
            Assert.Equal("ivan@mail.com", user.Email);
            Assert.Equal("hash:Password1", _uow.Users.GetById(user.Id).PasswordHash);
        }

        [Fact]
        public void Register_ExistingEmail_ThrowsConflict()
        {
            // Arrange
            var model = new RegisterInputModel { FullName = "Олена 2", Email = "OLENA@mail.com", Password = "Password1" };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Register(model));
        }

        [Fact]
        public void Register_ShortPassword_ThrowsValidation()
        {
            // Arrange
            var model = new RegisterInputModel { FullName = "Іван", Email = "ivan@mail.com", Password = "123" };

            // Act & Assert
            Assert.Throws<ValidationFailedException>(() => _service.Register(model));
        }

        [Fact]
        public void Login_CorrectPassword_ReturnsUserWithRole()
        {
            // Arrange
            var model = new LoginInputModel { Email = "manager@travel.ua", Password = "Manager123!" };

            // Act
            var user = _service.Login(model);

            // Assert
            Assert.Equal(UserRole.Manager, user.Role);
        }

        [Fact]
        public void Login_WrongPassword_ThrowsInvalidCredentials()
        {
            // Arrange
            var model = new LoginInputModel { Email = "olena@mail.com", Password = "wrong" };

            // Act & Assert
            Assert.Throws<InvalidCredentialsException>(() => _service.Login(model));
        }
    }
}
