using System.Collections.Generic;
using TravelAgency.BLL.Models.Countries;

namespace TravelAgency.BLL.Interfaces
{
    public interface ICountryService
    {
        IReadOnlyList<CountryModel> GetCountries();

        CountryModel CreateCountry(string name);

        RegionModel CreateRegion(int countryId, string name);
    }
}
