namespace TravelAgency.WebAPI.Models.Responses
{
    public class RegionResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }
    }
}
