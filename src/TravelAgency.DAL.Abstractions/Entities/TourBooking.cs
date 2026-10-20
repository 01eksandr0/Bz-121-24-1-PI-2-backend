using System;

namespace TravelAgency.DAL.Abstractions.Entities
{
    public class TourBooking : IBookingEntity
    {
        public int Id { get; set; }

        public int TourId { get; set; }

        public Tour Tour { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int Persons { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
