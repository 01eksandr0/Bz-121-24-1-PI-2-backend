using System.Linq;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Bookings;
using TravelAgency.BLL.Models.Hotels;
using TravelAgency.BLL.Models.Transport;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class HotelAndTransportServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();

        [Fact]
        public void GetAvailableRooms_ExcludesBookedAndTooSmallRooms()
        {
            // Arrange
            new RoomBookingService(_uow, new FixedClock()).Book(TestData.Olena, new BookRoomModel
            {
                RoomId = TestData.FamilyRoom, CheckIn = TestData.Today.AddDays(5), CheckOut = TestData.Today.AddDays(8), Guests = 3
            });
            var service = new HotelService(_uow, new FixedClock());

            // Act
            var forTwo = service.GetAvailableRooms(TestData.Hotel, TestData.Today.AddDays(6), TestData.Today.AddDays(7), 2);
            var forThree = service.GetAvailableRooms(TestData.Hotel, TestData.Today.AddDays(6), TestData.Today.AddDays(7), 3);

            // Assert
            Assert.Equal(TestData.DoubleRoom, Assert.Single(forTwo).Id);
            Assert.Empty(forThree);
        }

        [Fact]
        public void Search_ByMinStars_ReturnsHotelWithMinPrice()
        {
            // Arrange
            var service = new HotelService(_uow, new FixedClock());

            // Act
            var hotels = service.Search(new HotelFilterModel { MinStars = 4 });

            // Assert
            Assert.Equal(100m, Assert.Single(hotels).MinPricePerNight);
        }

        [Fact]
        public void AddRoom_InvalidPrice_ThrowsValidation()
        {
            // Arrange
            var service = new HotelService(_uow, new FixedClock());
            var input = new HotelRoomInputModel { Name = "Люкс", Capacity = 2, PricePerNight = 0 };

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => service.AddRoom(TestData.Hotel, input));
        }

        [Fact]
        public void SearchTrips_ByCityIgnoringCase_ReturnsFreeSeats()
        {
            // Arrange
            var service = new TransportService(_uow, new FixedClock());

            // Act
            var trips = service.Search(new TripFilterModel { ToCity = "анталія", Type = TransportKind.Plane });

            // Assert
            Assert.Equal(3, Assert.Single(trips).FreeSeats);
        }

        [Fact]
        public void CreateTrip_SameCities_ThrowsValidation()
        {
            // Arrange
            var service = new TransportService(_uow, new FixedClock());
            var input = new TripInputModel
            {
                Type = TransportKind.Bus, FromCity = "Київ", ToCity = "київ",
                DepartureAt = TestData.Today.AddDays(3), ArrivalAt = TestData.Today.AddDays(3).AddHours(5),
                Price = 20m, SeatsTotal = 40
            };

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => service.Create(input));
        }
    }
}
