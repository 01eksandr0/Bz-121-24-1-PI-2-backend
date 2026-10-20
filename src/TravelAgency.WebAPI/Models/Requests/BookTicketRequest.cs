using System.ComponentModel.DataAnnotations;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class BookTicketRequest
    {
        [Required]
        public int? TripId { get; set; }

        [Required]
        public int? Seats { get; set; }
    }
}
