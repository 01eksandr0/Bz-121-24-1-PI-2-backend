using System.ComponentModel.DataAnnotations;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class HotelRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int? Stars { get; set; }

        [Required]
        public int? RegionId { get; set; }
    }
}
