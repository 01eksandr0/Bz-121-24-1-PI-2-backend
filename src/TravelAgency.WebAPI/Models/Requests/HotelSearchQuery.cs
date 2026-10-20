namespace TravelAgency.WebAPI.Models.Requests
{
    public class HotelSearchQuery
    {
        public int? CountryId { get; set; }

        public int? RegionId { get; set; }

        public int? MinStars { get; set; }

        public string Text { get; set; }
    }
}
