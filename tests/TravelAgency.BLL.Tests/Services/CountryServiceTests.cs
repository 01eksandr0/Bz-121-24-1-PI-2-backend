using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Services;
using TravelAgency.BLL.Tests.Fakes;
using Xunit;

namespace TravelAgency.BLL.Tests.Services
{
    public class CountryServiceTests
    {
        private readonly CountryService _service = new CountryService(TestData.Create());

        [Fact]
        public void CreateCountry_DuplicateName_ThrowsConflict()
        {
            // Arrange
            const string sameNameOtherCase = "туреччина";

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.CreateCountry(sameNameOtherCase));
        }

        [Fact]
        public void CreateRegion_ReturnsRegionWithCountryName()
        {
            // Arrange
            const string regionName = "Бодрум";

            // Act
            var region = _service.CreateRegion(TestData.Turkey, regionName);

            // Assert
            Assert.Equal("Туреччина", region.CountryName);
        }
    }
}
