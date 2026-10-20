using System;

namespace TravelAgency.DAL.Abstractions.Entities
{
    /// <summary>Рейс літака, поїзда чи автобуса.</summary>
    public class TransportTrip : IEntity
    {
        public int Id { get; set; }

        public TransportType Type { get; set; }

        public string FromCity { get; set; }

        public string ToCity { get; set; }

        public DateTime DepartureAt { get; set; }

        public DateTime ArrivalAt { get; set; }

        public decimal Price { get; set; }

        public int SeatsTotal { get; set; }
    }
}
