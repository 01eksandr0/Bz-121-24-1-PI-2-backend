using System.ComponentModel.DataAnnotations;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class RoomRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int? Capacity { get; set; }

        /// <summary>Скільки однакових номерів цієї категорії є в готелі.</summary>
        [Required]
        public int? Quantity { get; set; }

        [Required]
        public decimal? PricePerNight { get; set; }
    }
}
