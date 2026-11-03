using System.ComponentModel.DataAnnotations;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class TourBookingRequest
    {
        [Required]
        public int? TourId { get; set; }

        [Required]
        public int? Persons { get; set; }
    }
}
