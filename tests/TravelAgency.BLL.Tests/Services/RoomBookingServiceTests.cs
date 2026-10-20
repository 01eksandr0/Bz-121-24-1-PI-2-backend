using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models.Bookings;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using TravelAgency.DAL.Abstractions.Entities;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class RoomBookingServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly RoomBookingService _service;

        public RoomBookingServiceTests()
        {
            _service = new RoomBookingService(_uow, new FixedClock());
        }

        private static BookRoomModel Request(int fromDay, int toDay, int guests = 2)
        {
            return new BookRoomModel
            {
                RoomId = TestData.DoubleRoom,
                CheckIn = TestData.Today.AddDays(fromDay),
                CheckOut = TestData.Today.AddDays(toDay),
                Guests = guests
            };
        }

        [Fact]
        public void Book_FreeRoom_CalculatesPriceByNights()
        {
            // Arrange
            var request = Request(5, 8);

            // Act
            var booking = _service.Book(TestData.Olena, request);

            // Assert
            Assert.Equal(3, booking.Nights);
            Assert.Equal(300m, booking.TotalPrice);
            Assert.Equal("Sea Hotel", booking.HotelName);
        }

        [Fact]
        public void Book_OverlappingDates_ThrowsConflict()
        {
            // Arrange
            _service.Book(TestData.Olena, Request(5, 8));

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => _service.Book(TestData.Petro, Request(7, 10)));
        }

        [Fact]
        public void Book_CheckInOnPreviousCheckOutDay_IsAllowed()
        {
            // Arrange
            _service.Book(TestData.Olena, Request(5, 8));

            // Act
            var second = _service.Book(TestData.Petro, Request(8, 10));

            // Assert
            Assert.Equal(2, second.Nights);
        }

        [Fact]
        public void Book_MoreGuestsThanCapacity_ThrowsValidation()
        {
            // Arrange
            var request = Request(5, 8, guests: 3);

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => _service.Book(TestData.Olena, request));
        }

        [Fact]
        public void Book_CheckOutBeforeCheckIn_ThrowsValidation()
        {
            // Arrange
            var request = Request(8, 5);

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => _service.Book(TestData.Olena, request));
        }

        [Fact]
        public void Cancel_OwnBooking_FreesRoomForSameDates()
        {
            // Arrange
            var booking = _service.Book(TestData.Olena, Request(5, 8));
            _service.Cancel(booking.Id, TestData.Olena);

            // Act
            var second = _service.Book(TestData.Petro, Request(5, 8));

            // Assert
            Assert.Equal(BookingStatus.Cancelled, _uow.RoomBookings.GetById(booking.Id).Status);
            Assert.Equal(TestData.Petro, second.UserId);
        }
    }
}
