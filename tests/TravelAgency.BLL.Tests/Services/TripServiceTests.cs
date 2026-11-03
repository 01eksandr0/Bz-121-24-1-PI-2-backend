using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Trips;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class TripServiceTests
    {
        private const int TooManySeats = 501;

        private readonly TripService _service = new TripService(TestData.Create(), new FixedClock());

        [Fact]
        public void Search_ByCityIgnoringCase_ReturnsFreePlaces()
        {
            // Arrange
            var filter = new TripSearchModel { ToCity = "анталія", Type = TransportType.Plane };

            // Act
            var trips = _service.Search(filter);

            // Assert
            Assert.Equal(3, Assert.Single(trips).FreePlaces);
        }

        [Fact]
        public void Create_SameCities_ThrowsValidation()
        {
            // Arrange
            var input = NewTrip("Київ", "київ", 40);

            // Act & Assert
            Assert.Throws<ValidationFailedException>(() => _service.Create(input));
        }

        [Fact]
        public void Create_CapacityAboveTripLimit_ThrowsValidation()
        {
            // Arrange
            var input = NewTrip("Київ", "Львів", TooManySeats);

            // Act & Assert
            Assert.Throws<ValidationFailedException>(() => _service.Create(input));
        }

        private static TripInputModel NewTrip(string from, string to, int capacity)
        {
            return new TripInputModel
            {
                Type = TransportType.Bus, FromCity = from, ToCity = to,
                DepartureAt = TestData.Today.AddDays(3), ArrivalAt = TestData.Today.AddDays(3).AddHours(5),
                Price = 20m, Capacity = capacity
            };
        }
    }
}
