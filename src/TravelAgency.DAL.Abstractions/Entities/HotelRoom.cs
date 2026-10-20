namespace TravelAgency.DAL.Abstractions.Entities
{
    /// <summary>Тип номера в готелі (кожен запис - окремий номер, який можна забронювати).</summary>
    public class HotelRoom : IEntity
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public decimal PricePerNight { get; set; }
    }
}
