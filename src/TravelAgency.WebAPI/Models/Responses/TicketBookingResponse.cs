using System;

namespace TravelAgency.WebAPI.Models.Responses
{
    public class TicketBookingResponse
    {
        public int Id { get; set; }

        public int TripId { get; set; }

        public string Route { get; set; }

        public string TransportType { get; set; }

        public DateTime DepartureAt { get; set; }

        public string UserName { get; set; }

        public int Seats { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
