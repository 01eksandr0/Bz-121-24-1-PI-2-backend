using System;

namespace TravelAgency.BLL.Models.Tours
{
    /// <summary>Фільтри і сортування для пошуку турів. Порожнє поле - не фільтрувати.</summary>
    public class TourSearchModel
    {
        public string Text { get; set; }

        public int? CountryId { get; set; }

        public int? RegionId { get; set; }

        public TourType? Type { get; set; }

        public bool HotOnly { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public bool OnlyAvailable { get; set; }

        public TourSortOrder SortBy { get; set; }
    }
}
