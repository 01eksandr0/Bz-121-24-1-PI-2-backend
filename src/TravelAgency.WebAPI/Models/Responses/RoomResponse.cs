namespace TravelAgency.WebAPI.Models.Responses
{
    public class RoomResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public int Quantity { get; set; }

        public decimal PricePerNight { get; set; }
    }
}
