using System.ComponentModel.DataAnnotations;

namespace TravelAgency.WebAPI.Models.Requests
{
    /// <summary>Запит з однією назвою (країна, регіон).</summary>
    public class NameRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
