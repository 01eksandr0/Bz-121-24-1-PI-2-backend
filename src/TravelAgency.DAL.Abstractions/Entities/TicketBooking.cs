using System;

namespace TravelAgency.DAL.Abstractions.Entities
{
    public class TicketBooking : IBookingEntity
    {
        public int Id { get; set; }

        public int TripId { get; set; }

        public TransportTrip Trip { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int Seats { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
