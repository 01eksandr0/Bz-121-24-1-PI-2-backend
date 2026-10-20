using System;

namespace TravelAgency.WebAPI.Models.Responses
{
    public class TripResponse
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string FromCity { get; set; }

        public string ToCity { get; set; }

        public DateTime DepartureAt { get; set; }

        public DateTime ArrivalAt { get; set; }

        public decimal Price { get; set; }

        public int SeatsTotal { get; set; }

        public int FreeSeats { get; set; }
    }
}
