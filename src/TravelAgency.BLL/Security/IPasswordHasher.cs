namespace TravelAgency.BLL.Security
{
    internal interface IPasswordHasher
    {
        string Hash(string password);

        bool Verify(string password, string hash);
    }
}
