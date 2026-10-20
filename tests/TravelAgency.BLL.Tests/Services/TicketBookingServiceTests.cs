using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models.Bookings;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class TicketBookingServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly TicketBookingService _service;

        public TicketBookingServiceTests()
        {
            _service = new TicketBookingService(_uow, new FixedClock());
        }

        [Fact]
        public void Book_FreeSeats_CalculatesTotalPrice()
        {
            // Arrange
            var request = new BookTicketModel { TripId = TestData.Flight, Seats = 2 };

            // Act
            var booking = _service.Book(TestData.Olena, request);

            // Assert
            Assert.Equal(300m, booking.TotalPrice);
            Assert.Equal("Київ - Анталія", booking.Route);
        }

        [Fact]
        public void Book_MoreSeatsThanLeft_ThrowsConflict()
        {
            // Arrange
            _service.Book(TestData.Olena, new BookTicketModel { TripId = TestData.Flight, Seats = 2 });

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() =>
                _service.Book(TestData.Petro, new BookTicketModel { TripId = TestData.Flight, Seats = 2 }));
        }

        [Fact]
        public void Cancel_OtherCustomersTicket_ThrowsAccessDenied()
        {
            // Arrange
            var booking = _service.Book(TestData.Olena, new BookTicketModel { TripId = TestData.Flight, Seats = 1 });

            // Act & Assert
            Assert.Throws<AccessDeniedException>(() => _service.Cancel(booking.Id, TestData.Petro));
        }
    }
}
