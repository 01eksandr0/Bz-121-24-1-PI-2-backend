using System.Collections.Generic;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Users;

namespace TravelAgency.BLL.Interfaces
{
    public interface IUserService
    {
        IReadOnlyList<UserModel> GetAll();

        UserModel GetById(int id);

        UserModel ChangeRole(int actingUserId, int userId, Role role);
    }
}
