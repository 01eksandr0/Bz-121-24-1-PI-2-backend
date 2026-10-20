using System.Linq;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Bookings;
using TravelAgency.BLL.Models.Tours;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using TravelAgency.DAL.Abstractions.Entities;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class TourServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly TourService _service;

        public TourServiceTests()
        {
            _service = new TourService(_uow, new FixedClock());
        }

        private static TourInputModel ValidInput()
        {
            return new TourInputModel
            {
                Title = "Новий тур до Анталії",
                Description = "Опис",
                Type = TourKind.Beach,
                RegionId = TestData.Antalya,
                StartDate = TestData.Today.AddDays(30),
                EndDate = TestData.Today.AddDays(37),
                PricePerPerson = 800m,
                DiscountPercent = 0,
                Capacity = 10
            };
        }

        [Fact]
        public void Search_NoFilters_ReturnsOnlyFutureToursSortedByStartDate()
        {
            // Arrange
            var filter = new TourFilterModel();

            // Act
            var result = _service.Search(filter);

            // Assert
            Assert.Equal(new[] { TestData.SmallTour, TestData.HotTour }, result.Select(t => t.Id).ToArray());
        }

        [Fact]
        public void Search_HotOnly_ReturnsDiscountedToursWithFinalPrice()
        {
            // Arrange
            var filter = new TourFilterModel { HotOnly = true };

            // Act
            var result = _service.Search(filter);

            // Assert
            var tour = Assert.Single(result);
            Assert.True(tour.IsHot);
            Assert.Equal(800m, tour.FinalPrice);
        }

        [Fact]
        public void Search_ByCountryTypeAndText_AppliesAllFilters()
        {
            // Arrange
            var filter = new TourFilterModel { CountryId = TestData.Turkey, Type = TourKind.Beach, Text = "ВКЛЮЧЕНО" };

            // Act
            var result = _service.Search(filter);

            // Assert
            Assert.Equal(TestData.HotTour, Assert.Single(result).Id);
        }

        [Fact]
        public void Search_MaxPrice_UsesPriceWithDiscount()
        {
            // Arrange
            var filter = new TourFilterModel { MaxPrice = 850m, SortBy = TourSortOrder.PriceDescending };

            // Act
            var result = _service.Search(filter);

            // Assert
            Assert.Equal(new[] { TestData.HotTour, TestData.SmallTour }, result.Select(t => t.Id).ToArray());
        }

        [Fact]
        public void Search_OnlyAvailable_HidesFullyBookedTour()
        {
            // Arrange
            _uow.TourBookings.Add(new TourBooking { TourId = TestData.SmallTour, UserId = TestData.Olena, Persons = 2, Status = BookingStatus.Active });
            var filter = new TourFilterModel { OnlyAvailable = true };

            // Act
            var result = _service.Search(filter);

            // Assert
            Assert.DoesNotContain(result, t => t.Id == TestData.SmallTour);
        }

        [Fact]
        public void GetById_Unknown_ThrowsNotFound()
        {
            // Arrange
            const int unknownId = 999;

            // Act & Assert
            Assert.Throws<EntityNotFoundException>(() => _service.GetById(unknownId));
        }

        [Fact]
        public void Create_ValidInput_SavesTourAndReturnsModel()
        {
            // Arrange
            var input = ValidInput();

            // Act
            var tour = _service.Create(input);

            // Assert
            Assert.Equal("Анталія", tour.RegionName);
            Assert.Equal(7, tour.Nights);
            Assert.Equal(10, tour.FreePlaces);
            Assert.Equal(1, _uow.SaveCount);
        }

        [Fact]
        public void Create_StartDateInPast_ThrowsValidationAndDoesNotSave()
        {
            // Arrange
            var input = ValidInput();
            input.StartDate = TestData.Today.AddDays(-1);

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => _service.Create(input));
            Assert.Equal(0, _uow.SaveCount);
        }

        [Fact]
        public void Create_EndBeforeStart_ThrowsValidation()
        {
            // Arrange
            var input = ValidInput();
            input.EndDate = input.StartDate;

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => _service.Create(input));
        }

        [Fact]
        public void Create_TooBigDiscount_ThrowsValidation()
        {
            // Arrange
            var input = ValidInput();
            input.DiscountPercent = 95;

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => _service.Create(input));
        }

        [Fact]
        public void Create_UnknownRegion_ThrowsNotFound()
        {
            // Arrange
            var input = ValidInput();
            input.RegionId = 999;

            // Act & Assert
            Assert.Throws<EntityNotFoundException>(() => _service.Create(input));
        }

        [Fact]
        public void Update_CapacityBelowBooked_ThrowsConflict()
        {
            // Arrange
            new TourBookingService(_uow, new FixedClock()).Book(TestData.Olena, new BookTourModel { TourId = TestData.HotTour, Persons = 5 });
            var input = ValidInput();
            input.Capacity = 4;

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => _service.Update(TestData.HotTour, input));
        }

        [Fact]
        public void Update_ValidInput_ChangesTour()
        {
            // Arrange
            var input = ValidInput();
            input.Title = "Анталія оновлена";
            input.DiscountPercent = 50;

            // Act
            var tour = _service.Update(TestData.HotTour, input);

            // Assert
            Assert.Equal("Анталія оновлена", tour.Title);
            Assert.Equal(400m, tour.FinalPrice);
        }

        [Fact]
        public void Delete_TourWithoutBookings_RemovesTour()
        {
            // Arrange
            int before = _uow.Tours.Count;

            // Act
            _service.Delete(TestData.SmallTour);

            // Assert
            Assert.Equal(before - 1, _uow.Tours.Count);
        }

        [Fact]
        public void Delete_TourWithBookings_ThrowsConflictAndKeepsTour()
        {
            // Arrange
            new TourBookingService(_uow, new FixedClock()).Book(TestData.Olena, new BookTourModel { TourId = TestData.HotTour, Persons = 1 });

            // Act & Assert
            Assert.Throws<BusinessConflictException>(() => _service.Delete(TestData.HotTour));
            Assert.NotNull(_uow.Tours.GetById(TestData.HotTour));
        }
    }
}
