using System;

namespace TravelAgency.WebAPI.Models.Responses
{
    public class RoomBookingResponse
    {
        public int Id { get; set; }

        public string HotelName { get; set; }

        public string RoomName { get; set; }

        public string UserName { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public int Nights { get; set; }

        public int Guests { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
