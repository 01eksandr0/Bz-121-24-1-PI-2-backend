using System.Collections.Generic;
using TravelAgency.BLL.Models.Geography;

namespace TravelAgency.BLL.Interfaces
{
    public interface IGeographyService
    {
        IReadOnlyList<CountryModel> GetCountries();

        CountryModel CreateCountry(string name);

        RegionModel CreateRegion(int countryId, string name);
    }
}
