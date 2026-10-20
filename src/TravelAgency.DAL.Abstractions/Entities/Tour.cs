using System;

namespace TravelAgency.DAL.Abstractions.Entities
{
    /// <summary>Тур. DiscountPercent &gt; 0 робить тур "гарячим".</summary>
    public class Tour : IEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TourType Type { get; set; }

        public int RegionId { get; set; }

        public Region Region { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal PricePerPerson { get; set; }

        public int DiscountPercent { get; set; }

        public int Capacity { get; set; }
    }
}
