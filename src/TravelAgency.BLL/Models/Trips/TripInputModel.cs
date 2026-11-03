using System;

namespace TravelAgency.BLL.Models.Trips
{
    public class TripInputModel
    {
        public TransportType Type { get; set; }

        public string FromCity { get; set; }

        public string ToCity { get; set; }

        public DateTime DepartureAt { get; set; }

        public DateTime ArrivalAt { get; set; }

        public decimal Price { get; set; }

        public int Capacity { get; set; }
    }
}
