using System;

namespace TravelAgency.BLL.Models.Transport
{
    public class TripFilterModel
    {
        public TransportKind? Type { get; set; }

        public string FromCity { get; set; }

        public string ToCity { get; set; }

        public DateTime? Date { get; set; }
    }
}
