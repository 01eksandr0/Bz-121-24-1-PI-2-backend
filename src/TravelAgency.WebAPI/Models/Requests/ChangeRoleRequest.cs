using System.ComponentModel.DataAnnotations;
using TravelAgency.BLL.Models;

namespace TravelAgency.WebAPI.Models.Requests
{
    /// <summary>Нова роль: Customer, Manager або Admin.</summary>
    public class ChangeRoleRequest
    {
        [Required, EnumDataType(typeof(Role))]
        public string Role { get; set; }
    }
}
