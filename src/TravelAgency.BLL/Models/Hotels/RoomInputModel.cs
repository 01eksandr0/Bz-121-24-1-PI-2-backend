namespace TravelAgency.BLL.Models.Hotels
{
    public class RoomInputModel
    {
        public string Name { get; set; }

        public int Capacity { get; set; }

        public int Quantity { get; set; }

        public decimal PricePerNight { get; set; }
    }
}
