namespace TravelAgency.WebAPI.Models.Responses
{
    public class HotelRoomResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public decimal PricePerNight { get; set; }
    }
}
