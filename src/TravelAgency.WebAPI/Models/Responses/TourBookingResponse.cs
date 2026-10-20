using System;

namespace TravelAgency.WebAPI.Models.Responses
{
    public class TourBookingResponse
    {
        public int Id { get; set; }

        public int TourId { get; set; }

        public string TourTitle { get; set; }

        public DateTime TourStartDate { get; set; }

        public string UserName { get; set; }

        public int Persons { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
