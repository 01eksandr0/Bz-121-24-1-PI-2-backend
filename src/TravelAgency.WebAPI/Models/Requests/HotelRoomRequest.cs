using System.ComponentModel.DataAnnotations;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class HotelRoomRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int? Capacity { get; set; }

        [Required]
        public decimal? PricePerNight { get; set; }
    }
}
