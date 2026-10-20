namespace TravelAgency.DAL.Abstractions.Entities
{
    /// <summary>Регіон або місто країни.</summary>
    public class Region : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }
    }
}
