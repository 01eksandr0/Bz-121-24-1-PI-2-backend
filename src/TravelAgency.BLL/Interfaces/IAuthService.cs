using TravelAgency.BLL.Models.Users;

namespace TravelAgency.BLL.Interfaces
{
    /// <summary>Реєстрація і перевірка облікових даних. Токен видає рівень представлення.</summary>
    public interface IAuthService
    {
        UserModel Register(RegisterModel model);

        UserModel Login(LoginModel model);
    }
}
