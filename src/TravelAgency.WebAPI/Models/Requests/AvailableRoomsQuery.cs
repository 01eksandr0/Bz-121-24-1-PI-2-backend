using System;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class AvailableRoomsQuery
    {
        [Required]
        public DateTime? CheckIn { get; set; }

        [Required]
        public DateTime? CheckOut { get; set; }

        public int Guests { get; set; } = 1;
    }
}
