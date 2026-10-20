using System;

namespace TravelAgency.BLL.Models.Tours
{
    /// <summary>Тур для показу: з обчисленою ціною зі знижкою і кількістю вільних місць.</summary>
    public class TourModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TourKind Type { get; set; }

        public int RegionId { get; set; }

        public string RegionName { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Nights { get; set; }

        public decimal PricePerPerson { get; set; }

        public int DiscountPercent { get; set; }

        public decimal FinalPrice { get; set; }

        public bool IsHot { get; set; }

        public int Capacity { get; set; }

        public int FreePlaces { get; set; }
    }
}
