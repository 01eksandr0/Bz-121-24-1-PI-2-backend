using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Models.Users;
using TravelAgency.WebAPI.Mapping;
using TravelAgency.WebAPI.Models.Requests;
using TravelAgency.WebAPI.Models.Responses;
using TravelAgency.WebAPI.Security;

namespace TravelAgency.WebAPI.Controllers
{
    /// <summary>Реєстрація, вхід і дані поточного користувача.</summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IUserService _users;
        private readonly IJwtTokenService _tokens;

        public AuthController(IAuthService auth, IUserService users, IJwtTokenService tokens)
        {
            _auth = auth;
            _users = users;
            _tokens = tokens;
        }

        /// <summary>POST api/auth/register - новий клієнт; одразу повертає токен.</summary>
        [HttpPost("register")]
        public ActionResult<AuthResponse> Register([FromBody] RegisterRequest request)
        {
            return CreateAuthResponse(_auth.Register(request.ToModel()));
        }

        /// <summary>POST api/auth/login</summary>
        [HttpPost("login")]
        public ActionResult<AuthResponse> Login([FromBody] LoginRequest request)
        {
            return CreateAuthResponse(_auth.Login(request.ToModel()));
        }

        /// <summary>GET api/auth/me - хто я (за токеном).</summary>
        [HttpGet("me")]
        [Authorize]
        public ActionResult<UserResponse> Me()
        {
            return _users.GetById(User.GetUserId()).ToResponse();
        }

        private AuthResponse CreateAuthResponse(UserModel user)
        {
            AccessToken token = _tokens.Create(user);
            return new AuthResponse { Token = token.Value, ExpiresAt = token.ExpiresAt, User = user.ToResponse() };
        }
    }
}
