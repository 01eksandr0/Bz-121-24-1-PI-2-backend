using System.Collections.Generic;

namespace TravelAgency.WebAPI.Models.Responses
{
    public class HotelResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Stars { get; set; }

        public int RegionId { get; set; }

        public string RegionName { get; set; }

        public string CountryName { get; set; }

        public decimal MinPricePerNight { get; set; }

        public IReadOnlyList<HotelRoomResponse> Rooms { get; set; }
    }
}
