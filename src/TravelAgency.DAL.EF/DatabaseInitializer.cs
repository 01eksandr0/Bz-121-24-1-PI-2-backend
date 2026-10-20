using Microsoft.EntityFrameworkCore;

namespace TravelAgency.DAL.EF
{
    /// <summary>Налаштування підключення до MS SQL і створення бази з тестовими даними.</summary>
    public static class DatabaseInitializer
    {
        public static DbContextOptions<TravelAgencyContext> CreateOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<TravelAgencyContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        /// <summary>Якщо бази немає - створює її за моделлю (Code First) і заповнює демо-даними.</summary>
        public static void EnsureCreated(DbContextOptions<TravelAgencyContext> options)
        {
            using (var context = new TravelAgencyContext(options))
            {
                if (context.Database.EnsureCreated())
                {
                    DemoDataSeeder.Seed(context);
                }
            }
        }
    }
}
