using System;

namespace TravelAgency.BLL.Models.Trips
{
    public class TripSearchModel
    {
        public TransportType? Type { get; set; }

        public string FromCity { get; set; }

        public string ToCity { get; set; }

        public DateTime? Date { get; set; }
    }
}
