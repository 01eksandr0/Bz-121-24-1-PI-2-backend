using System;

namespace TravelAgency.DAL.Abstractions.Entities
{
    public class RoomBooking : IBookingEntity
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public HotelRoom Room { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public int Guests { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
