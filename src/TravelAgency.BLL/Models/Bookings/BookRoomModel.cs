using System;

namespace TravelAgency.BLL.Models.Bookings
{
    public class BookRoomModel
    {
        public int RoomId { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public int Guests { get; set; }
    }
}
