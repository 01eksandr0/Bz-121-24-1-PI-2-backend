using System;

namespace TravelAgency.BLL.Models.Bookings
{
    public class RoomBookingModel
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public string RoomName { get; set; }

        public string HotelName { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public int Nights { get; set; }

        public int Guests { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingState Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
