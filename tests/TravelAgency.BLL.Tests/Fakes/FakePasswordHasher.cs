using TravelAgency.BLL.Security;

namespace TravelAgency.BLL.Tests.Fakes
{
    /// <summary>Stub хешера: "хеш" - це пароль з префіксом. Швидко і передбачувано.</summary>
    internal class FakePasswordHasher : IPasswordHasher
    {
        private const string Prefix = "hash:";

        public string Hash(string password) => Prefix + password;

        public bool Verify(string password, string hash) => hash == Prefix + password;
    }
}
