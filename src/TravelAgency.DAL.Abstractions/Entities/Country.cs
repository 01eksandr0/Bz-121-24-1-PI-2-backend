namespace TravelAgency.DAL.Abstractions.Entities
{
    public class Country : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
