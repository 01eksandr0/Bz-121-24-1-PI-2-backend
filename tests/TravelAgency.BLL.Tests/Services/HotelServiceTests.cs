using System;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models.Bookings;
using TravelAgency.BLL.Models.Hotels;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class HotelServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly HotelService _service;

        public HotelServiceTests()
        {
            _service = new HotelService(_uow, new FixedClock());
        }

        [Fact]
        public void GetAvailableRooms_ExcludesBookedAndTooSmallRooms()
        {
            // Arrange
            BookFamilyRoom(TestData.Today.AddDays(5), TestData.Today.AddDays(8));

            // Act
            var forTwo = _service.GetAvailableRooms(TestData.Hotel, TestData.Today.AddDays(6), TestData.Today.AddDays(7), 2);
            var forThree = _service.GetAvailableRooms(TestData.Hotel, TestData.Today.AddDays(6), TestData.Today.AddDays(7), 3);

            // Assert
            Assert.Equal(TestData.DoubleRoom, Assert.Single(forTwo).Id);
            Assert.Empty(forThree);
        }

        [Fact]
        public void GetAvailableRooms_CategoryWithFreeRoomsLeft_IsStillAvailable()
        {
            // Arrange
            _uow.Rooms.Raw(TestData.FamilyRoom).Quantity = 2;
            BookFamilyRoom(TestData.Today.AddDays(5), TestData.Today.AddDays(8));

            // Act
            var rooms = _service.GetAvailableRooms(TestData.Hotel, TestData.Today.AddDays(6), TestData.Today.AddDays(7), 3);

            // Assert
            Assert.Equal(TestData.FamilyRoom, Assert.Single(rooms).Id);
        }

        [Fact]
        public void GetAvailableRooms_ZeroGuests_ThrowsValidation()
        {
            // Arrange
            const int guests = 0;

            // Act & Assert
            Assert.Throws<ValidationFailedException>(() =>
                _service.GetAvailableRooms(TestData.Hotel, TestData.Today.AddDays(6), TestData.Today.AddDays(7), guests));
        }

        [Fact]
        public void Search_ByMinStars_ReturnsHotelWithMinPrice()
        {
            // Arrange
            var filter = new HotelSearchModel { MinStars = 4 };

            // Act
            var hotels = _service.Search(filter);

            // Assert
            Assert.Equal(100m, Assert.Single(hotels).MinPricePerNight);
        }

        [Fact]
        public void AddRoom_InvalidPrice_ThrowsValidation()
        {
            // Arrange
            var input = new RoomInputModel { Name = "Люкс", Capacity = 2, Quantity = 3, PricePerNight = 0 };

            // Act & Assert
            Assert.Throws<ValidationFailedException>(() => _service.AddRoom(TestData.Hotel, input));
        }

        [Fact]
        public void AddRoom_ZeroQuantity_ThrowsValidation()
        {
            // Arrange
            var input = new RoomInputModel { Name = "Люкс", Capacity = 2, Quantity = 0, PricePerNight = 300m };

            // Act & Assert
            Assert.Throws<ValidationFailedException>(() => _service.AddRoom(TestData.Hotel, input));
        }

        private void BookFamilyRoom(DateTime checkIn, DateTime checkOut)
        {
            new RoomBookingService(_uow, new FixedClock()).Book(TestData.Olena, new RoomBookingInputModel
            {
                RoomId = TestData.FamilyRoom, CheckIn = checkIn, CheckOut = checkOut, Guests = 3
            });
        }
    }
}
