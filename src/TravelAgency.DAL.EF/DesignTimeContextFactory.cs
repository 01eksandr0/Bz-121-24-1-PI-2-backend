using System;
using Microsoft.EntityFrameworkCore.Design;

namespace TravelAgency.DAL.EF
{
    /// <summary>
    /// Потрібна лише інструменту dotnet-ef для створення міграцій.
    /// Рядок підключення - зі змінної середовища (як у ASP.NET Core), інакше - SQL Server з docker-compose.yml.
    /// </summary>
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<TravelAgencyContext>
    {
        private const string ConnectionVariable = "ConnectionStrings__TravelAgency";
        private const string DockerConnection =
            "Server=localhost,1433;Database=TravelAgencyDb;User Id=sa;Password=TravelAgency!2026;TrustServerCertificate=True";

        public TravelAgencyContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable(ConnectionVariable) ?? DockerConnection;
            return new TravelAgencyContext(DatabaseInitializer.CreateOptions(connectionString));
        }
    }
}
