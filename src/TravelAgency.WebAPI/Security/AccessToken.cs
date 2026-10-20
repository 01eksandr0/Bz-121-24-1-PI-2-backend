using System;

namespace TravelAgency.WebAPI.Security
{
    public class AccessToken
    {
        public AccessToken(string value, DateTime expiresAt)
        {
            Value = value;
            ExpiresAt = expiresAt;
        }

        public string Value { get; }

        public DateTime ExpiresAt { get; }
    }
}
