using System.Collections.Generic;

namespace TravelAgency.BLL.Models.Hotels
{
    public class HotelModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Stars { get; set; }

        public int RegionId { get; set; }

        public string RegionName { get; set; }

        public string CountryName { get; set; }

        public decimal MinPricePerNight { get; set; }

        public IReadOnlyList<HotelRoomModel> Rooms { get; set; } = new List<HotelRoomModel>();
    }
}
