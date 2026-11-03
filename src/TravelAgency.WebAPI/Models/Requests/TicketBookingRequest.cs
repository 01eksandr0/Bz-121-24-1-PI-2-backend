using System.ComponentModel.DataAnnotations;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class TicketBookingRequest
    {
        [Required]
        public int? TripId { get; set; }

        [Required]
        public int? Seats { get; set; }
    }
}
