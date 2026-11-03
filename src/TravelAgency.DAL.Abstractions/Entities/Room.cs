namespace TravelAgency.DAL.Abstractions.Entities
{
    /// <summary>
    /// Категорія номерів у готелі ("Стандарт", "Сімейний"). Quantity - скільки однакових номерів цієї категорії є,
    /// тобто скільки бронювань на ті самі дати можна прийняти.
    /// </summary>
    public class Room : IEntity
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public int Quantity { get; set; }

        public decimal PricePerNight { get; set; }
    }
}
