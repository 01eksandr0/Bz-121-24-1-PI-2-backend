using System;

namespace TravelAgency.WebAPI.Models.Responses
{
    /// <summary>Відповідь на вхід/реєстрацію: JWT-токен і дані користувача.</summary>
    public class AuthResponse
    {
        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public UserResponse User { get; set; }
    }
}
