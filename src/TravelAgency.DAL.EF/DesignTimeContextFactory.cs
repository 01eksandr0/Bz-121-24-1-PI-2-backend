using Microsoft.EntityFrameworkCore.Design;

namespace TravelAgency.DAL.EF
{
    /// <summary>Потрібна лише інструменту dotnet-ef для створення міграцій.</summary>
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<TravelAgencyContext>
    {
        private const string LocalDbConnection =
            "Server=(localdb)\\MSSQLLocalDB;Database=TravelAgencyDb;Trusted_Connection=True;TrustServerCertificate=True";

        public TravelAgencyContext CreateDbContext(string[] args)
        {
            return new TravelAgencyContext(DatabaseInitializer.CreateOptions(LocalDbConnection));
        }
    }
}
