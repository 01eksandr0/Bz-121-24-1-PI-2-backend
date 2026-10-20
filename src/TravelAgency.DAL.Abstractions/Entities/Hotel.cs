using System.Collections.Generic;

namespace TravelAgency.DAL.Abstractions.Entities
{
    public class Hotel : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Stars { get; set; }

        public int RegionId { get; set; }

        public Region Region { get; set; }

        public ICollection<HotelRoom> Rooms { get; set; } = new List<HotelRoom>();
    }
}
