using TravelAgency.BLL.Models.Users;

namespace TravelAgency.WebAPI.Security
{
    /// <summary>Видача токена - задача рівня представлення: BLL лише перевіряє облікові дані.</summary>
    public interface IJwtTokenService
    {
        AccessToken Create(UserModel user);
    }
}
