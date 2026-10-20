using System;
using System.ComponentModel.DataAnnotations;
using TravelAgency.BLL.Models;

namespace TravelAgency.WebAPI.Models.Requests
{
    /// <summary>Створення / редагування туру. Числа nullable, щоб [Required] помітив відсутнє поле.</summary>
    public class TourRequest
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required, EnumDataType(typeof(TourKind))]
        public string Type { get; set; }

        [Required]
        public int? RegionId { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        [Required]
        public decimal? PricePerPerson { get; set; }

        public int DiscountPercent { get; set; }

        [Required]
        public int? Capacity { get; set; }
    }
}
