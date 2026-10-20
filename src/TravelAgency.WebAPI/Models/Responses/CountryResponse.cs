using System.Collections.Generic;

namespace TravelAgency.WebAPI.Models.Responses
{
    public class CountryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IReadOnlyList<RegionResponse> Regions { get; set; }
    }
}
