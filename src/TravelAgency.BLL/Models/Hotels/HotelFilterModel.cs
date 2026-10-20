namespace TravelAgency.BLL.Models.Hotels
{
    public class HotelFilterModel
    {
        public int? CountryId { get; set; }

        public int? RegionId { get; set; }

        public int? MinStars { get; set; }

        public string Text { get; set; }
    }
}
