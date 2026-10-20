using System;
using System.ComponentModel.DataAnnotations;
using TravelAgency.BLL.Models;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class TripSearchQuery
    {
        [EnumDataType(typeof(TransportKind))]
        public string Type { get; set; }

        public string FromCity { get; set; }

        public string ToCity { get; set; }

        public DateTime? Date { get; set; }
    }
}
