using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Users;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class AuthAndUserServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly AuthService _auth;

        public AuthAndUserServiceTests()
        {
            _auth = new AuthService(_uow, new FakePasswordHasher());
        }

        [Fact]
        public void Register_NewUser_GetsCustomerRoleAndHashedPassword()
        {
            // Arrange
            var model = new RegisterModel { FullName = "Іван", Email = " Ivan@Mail.com ", Password = "Password1" };

            // Act
            var user = _auth.Register(model);

            // Assert
            Assert.Equal(Role.Customer, user.Role);
            Assert.Equal("ivan@mail.com", user.Email);
            Assert.Equal("hash:Password1", _uow.Users.GetById(user.Id).PasswordHash);
        }

        [Fact]
        public void Register_ExistingEmail_ThrowsConflict()
        {
            // Arrange
            var model = new RegisterModel { FullName = "Олена 2", Email = "OLENA@mail.com", Password = "Password1" };

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => _auth.Register(model));
        }

        [Fact]
        public void Register_ShortPassword_ThrowsValidation()
        {
            // Arrange
            var model = new RegisterModel { FullName = "Іван", Email = "ivan@mail.com", Password = "123" };

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => _auth.Register(model));
        }

        [Fact]
        public void Login_CorrectPassword_ReturnsUserWithRole()
        {
            // Arrange
            var model = new LoginModel { Email = "manager@travel.ua", Password = "Manager123!" };

            // Act
            var user = _auth.Login(model);

            // Assert
            Assert.Equal(Role.Manager, user.Role);
        }

        [Fact]
        public void Login_WrongPassword_ThrowsInvalidCredentials()
        {
            // Arrange
            var model = new LoginModel { Email = "olena@mail.com", Password = "wrong" };

            // Act & Assert
            Assert.Throws<InvalidCredentialsException>(() => _auth.Login(model));
        }

        [Fact]
        public void ChangeRole_AdminPromotesCustomer_SavesNewRole()
        {
            // Arrange
            var service = new UserService(_uow);

            // Act
            var user = service.ChangeRole(TestData.Admin, TestData.Olena, Role.Manager);

            // Assert
            Assert.Equal(Role.Manager, user.Role);
            Assert.Equal(1, _uow.SaveCount);
        }

        [Fact]
        public void ChangeRole_OwnRole_ThrowsAccessDenied()
        {
            // Arrange
            var service = new UserService(_uow);

            // Act & Assert
            Assert.Throws<AccessDeniedException>(() => service.ChangeRole(TestData.Admin, TestData.Admin, Role.Customer));
        }

        [Fact]
        public void CreateCountry_DuplicateName_ThrowsConflict()
        {
            // Arrange
            var service = new GeographyService(_uow);

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => service.CreateCountry("туреччина"));
        }

        [Fact]
        public void CreateRegion_ReturnsRegionWithCountryName()
        {
            // Arrange
            var service = new GeographyService(_uow);

            // Act
            var region = service.CreateRegion(TestData.Turkey, "Бодрум");

            // Assert
            Assert.Equal("Туреччина", region.CountryName);
        }
    }
}
