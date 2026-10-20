using System;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class BookRoomRequest
    {
        [Required]
        public int? RoomId { get; set; }

        [Required]
        public DateTime? CheckIn { get; set; }

        [Required]
        public DateTime? CheckOut { get; set; }

        [Required]
        public int? Guests { get; set; }
    }
}
