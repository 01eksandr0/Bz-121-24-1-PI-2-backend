using System;

namespace TravelAgency.BLL.Models.Bookings
{
    public class TicketBookingModel
    {
        public int Id { get; set; }

        public int TripId { get; set; }

        public string Route { get; set; }

        public TransportKind TransportType { get; set; }

        public DateTime DepartureAt { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int Seats { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingState Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
