using System;
using System.ComponentModel.DataAnnotations;
using TravelAgency.BLL.Models;

namespace TravelAgency.WebAPI.Models.Requests
{
    public class TripRequest
    {
        [Required, EnumDataType(typeof(TransportKind))]
        public string Type { get; set; }

        [Required]
        public string FromCity { get; set; }

        [Required]
        public string ToCity { get; set; }

        [Required]
        public DateTime? DepartureAt { get; set; }

        [Required]
        public DateTime? ArrivalAt { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public int? SeatsTotal { get; set; }
    }
}
