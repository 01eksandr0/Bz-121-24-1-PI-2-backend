using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.BLL.Interfaces;
using TravelAgency.WebAPI.Mapping;
using TravelAgency.WebAPI.Models.Requests;
using TravelAgency.WebAPI.Models.Responses;
using TravelAgency.WebAPI.Security;

namespace TravelAgency.WebAPI.Controllers
{
    /// <summary>Керування користувачами - тільки адміністратор.</summary>
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = ApiRoles.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _users;

        public UsersController(IUserService users)
        {
            _users = users;
        }

        /// <summary>GET api/users</summary>
        [HttpGet]
        public ActionResult<List<UserResponse>> GetAll()
        {
            return _users.GetAll().ToResponses(u => u.ToResponse());
        }

        /// <summary>PUT api/users/5/role - призначити роль (наприклад, зробити менеджером).</summary>
        [HttpPut("{id:int}/role")]
        public ActionResult<UserResponse> ChangeRole(int id, [FromBody] ChangeRoleRequest request)
        {
            return _users.ChangeRole(User.GetUserId(), id, request.ToRole()).ToResponse();
        }
    }
}
