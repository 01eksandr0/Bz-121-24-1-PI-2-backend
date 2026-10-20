using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Bookings;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using TravelAgency.DAL.Abstractions.Entities;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class TourBookingServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly TourBookingService _service;

        public TourBookingServiceTests()
        {
            _service = new TourBookingService(_uow, new FixedClock());
        }

        private TourBookingModel BookHotTour(int userId, int persons)
        {
            return _service.Book(userId, new BookTourModel { TourId = TestData.HotTour, Persons = persons });
        }

        [Fact]
        public void Book_ValidRequest_CreatesActiveBookingWithDiscountedPrice()
        {
            // Arrange
            var request = new BookTourModel { TourId = TestData.HotTour, Persons = 2 };

            // Act
            var booking = _service.Book(TestData.Olena, request);

            // Assert
            Assert.Equal(BookingState.Active, booking.Status);
            Assert.Equal(1600m, booking.TotalPrice);
            Assert.Equal("Олена", booking.UserName);
            Assert.Equal(1, _uow.SaveCount);
        }

        [Fact]
        public void Book_UnregisteredUser_ThrowsAccessDenied()
        {
            // Arrange
            var request = new BookTourModel { TourId = TestData.HotTour, Persons = 1 };

            // Act & Assert
            Assert.Throws<AccessDeniedException>(() => _service.Book(TestData.UnknownUser, request));
        }

        [Fact]
        public void Book_MorePersonsThanFreePlaces_ThrowsConflict()
        {
            // Arrange
            var request = new BookTourModel { TourId = TestData.SmallTour, Persons = 3 };

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => _service.Book(TestData.Olena, request));
        }

        [Fact]
        public void Book_PastTour_ThrowsConflict()
        {
            // Arrange
            var request = new BookTourModel { TourId = TestData.PastTour, Persons = 1 };

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => _service.Book(TestData.Olena, request));
        }

        [Fact]
        public void Book_SecondActiveBookingOfSameTour_ThrowsConflict()
        {
            // Arrange
            BookHotTour(TestData.Olena, 1);

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => BookHotTour(TestData.Olena, 1));
        }

        [Fact]
        public void Book_ZeroPersons_ThrowsValidation()
        {
            // Arrange
            var request = new BookTourModel { TourId = TestData.HotTour, Persons = 0 };

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => _service.Book(TestData.Olena, request));
        }

        [Fact]
        public void GetByUser_ReturnsOnlyUsersBookings()
        {
            // Arrange
            BookHotTour(TestData.Olena, 1);
            BookHotTour(TestData.Petro, 1);

            // Act
            var result = _service.GetByUser(TestData.Olena);

            // Assert
            Assert.Equal(TestData.Olena, Assert.Single(result).UserId);
        }

        [Fact]
        public void GetAll_ReturnsBookingsOfAllUsers()
        {
            // Arrange
            BookHotTour(TestData.Olena, 1);
            BookHotTour(TestData.Petro, 1);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Cancel_OwnBookingBeforeDeadline_SetsCancelled()
        {
            // Arrange
            var booking = BookHotTour(TestData.Olena, 1);

            // Act
            _service.Cancel(booking.Id, TestData.Olena);

            // Assert
            Assert.Equal(BookingStatus.Cancelled, _uow.TourBookings.GetById(booking.Id).Status);
        }

        [Fact]
        public void Cancel_OtherCustomersBooking_ThrowsAccessDenied()
        {
            // Arrange
            var booking = BookHotTour(TestData.Olena, 1);

            // Act & Assert
            Assert.Throws<AccessDeniedException>(() => _service.Cancel(booking.Id, TestData.Petro));
        }

        [Fact]
        public void Cancel_CustomerAfterDeadline_ThrowsConflict()
        {
            // Arrange: тур через 2 дні, а скасувати можна не пізніше ніж за 3 дні
            var booking = _service.Book(TestData.Olena, new BookTourModel { TourId = TestData.SmallTour, Persons = 1 });

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => _service.Cancel(booking.Id, TestData.Olena));
        }

        [Fact]
        public void Cancel_ManagerAfterCustomerDeadline_IsAllowed()
        {
            // Arrange
            var booking = _service.Book(TestData.Olena, new BookTourModel { TourId = TestData.SmallTour, Persons = 1 });

            // Act
            _service.Cancel(booking.Id, TestData.Manager);

            // Assert
            Assert.Equal(BookingStatus.Cancelled, _uow.TourBookings.GetById(booking.Id).Status);
        }

        [Fact]
        public void Cancel_AlreadyCancelled_ThrowsConflict()
        {
            // Arrange
            var booking = BookHotTour(TestData.Olena, 1);
            _service.Cancel(booking.Id, TestData.Olena);

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => _service.Cancel(booking.Id, TestData.Olena));
        }

        [Fact]
        public void Cancel_FreesPlacesForOtherCustomers()
        {
            // Arrange
            var booking = _service.Book(TestData.Olena, new BookTourModel { TourId = TestData.SmallTour, Persons = 2 });
            _service.Cancel(booking.Id, TestData.Manager);

            // Act
            var second = _service.Book(TestData.Petro, new BookTourModel { TourId = TestData.SmallTour, Persons = 2 });

            // Assert
            Assert.Equal(BookingState.Active, second.Status);
        }
    }
}
