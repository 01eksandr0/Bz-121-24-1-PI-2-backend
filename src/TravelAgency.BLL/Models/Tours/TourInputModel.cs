using System;

namespace TravelAgency.BLL.Models.Tours
{
    /// <summary>Дані для створення і редагування туру (менеджер).</summary>
    public class TourInputModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public TourKind Type { get; set; }

        public int RegionId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal PricePerPerson { get; set; }

        public int DiscountPercent { get; set; }

        public int Capacity { get; set; }
    }
}
