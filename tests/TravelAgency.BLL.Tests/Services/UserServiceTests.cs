using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class UserServiceTests
    {
        private const UserRole UnknownRole = (UserRole)99;

        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly UserService _service;

        public UserServiceTests()
        {
            _service = new UserService(_uow);
        }

        [Fact]
        public void ChangeRole_AdminPromotesCustomer_SavesNewRole()
        {
            // Arrange
            var newRole = UserRole.Manager;

            // Act
            var user = _service.ChangeRole(TestData.Admin, TestData.Olena, newRole);

            // Assert
            Assert.Equal(UserRole.Manager, user.Role);
            Assert.Equal(1, _uow.SaveCount);
        }

        [Fact]
        public void ChangeRole_OwnRole_ThrowsAccessDenied()
        {
            // Arrange
            var newRole = UserRole.Customer;

            // Act & Assert
            Assert.Throws<AccessDeniedException>(() => _service.ChangeRole(TestData.Admin, TestData.Admin, newRole));
        }

        [Fact]
        public void ChangeRole_UnknownRole_ThrowsValidationAndDoesNotSave()
        {
            // Arrange
            var newRole = UnknownRole;

            // Act
            Assert.Throws<ValidationFailedException>(() => _service.ChangeRole(TestData.Admin, TestData.Olena, newRole));

            // Assert
            Assert.Equal(0, _uow.SaveCount);
        }
    }
}
