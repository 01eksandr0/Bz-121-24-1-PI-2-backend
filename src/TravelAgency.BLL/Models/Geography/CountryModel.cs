using System.Collections.Generic;

namespace TravelAgency.BLL.Models.Geography
{
    public class CountryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IReadOnlyList<RegionModel> Regions { get; set; } = new List<RegionModel>();
    }
}
