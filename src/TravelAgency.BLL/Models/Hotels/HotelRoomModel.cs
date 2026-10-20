namespace TravelAgency.BLL.Models.Hotels
{
    public class HotelRoomModel
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public decimal PricePerNight { get; set; }
    }
}
