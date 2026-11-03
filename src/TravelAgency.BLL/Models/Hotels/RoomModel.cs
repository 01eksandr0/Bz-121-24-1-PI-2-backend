namespace TravelAgency.BLL.Models.Hotels
{
    public class RoomModel
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public int Quantity { get; set; }

        public decimal PricePerNight { get; set; }
    }
}
