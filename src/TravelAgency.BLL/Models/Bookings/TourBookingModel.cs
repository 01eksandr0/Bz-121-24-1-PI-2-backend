using System;

namespace TravelAgency.BLL.Models.Bookings
{
    public class TourBookingModel
    {
        public int Id { get; set; }

        public int TourId { get; set; }

        public string TourTitle { get; set; }

        public DateTime TourStartDate { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int Persons { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingState Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
