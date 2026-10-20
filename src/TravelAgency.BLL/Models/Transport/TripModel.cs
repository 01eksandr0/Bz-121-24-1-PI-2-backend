using System;

namespace TravelAgency.BLL.Models.Transport
{
    public class TripModel
    {
        public int Id { get; set; }

        public TransportKind Type { get; set; }

        public string FromCity { get; set; }

        public string ToCity { get; set; }

        public DateTime DepartureAt { get; set; }

        public DateTime ArrivalAt { get; set; }

        public decimal Price { get; set; }

        public int SeatsTotal { get; set; }

        public int FreeSeats { get; set; }
    }
}
