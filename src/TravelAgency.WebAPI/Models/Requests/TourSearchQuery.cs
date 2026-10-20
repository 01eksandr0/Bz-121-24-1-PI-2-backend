using System;
using System.ComponentModel.DataAnnotations;
using TravelAgency.BLL.Models;

namespace TravelAgency.WebAPI.Models.Requests
{
    /// <summary>Параметри рядка запиту GET /api/tours.</summary>
    public class TourSearchQuery
    {
        public string Text { get; set; }

        public int? CountryId { get; set; }

        public int? RegionId { get; set; }

        [EnumDataType(typeof(TourKind))]
        public string Type { get; set; }

        public bool HotOnly { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public bool OnlyAvailable { get; set; }

        [EnumDataType(typeof(TourSortOrder))]
        public string SortBy { get; set; }
    }
}
