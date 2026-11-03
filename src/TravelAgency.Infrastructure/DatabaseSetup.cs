using TravelAgency.DAL.EF;

namespace TravelAgency.Infrastructure
{
    /// <summary>Підготовка бази під час старту програми (WebAPI не знає про EF, тому викликає її звідси).</summary>
    public static class DatabaseSetup
    {
        /// <summary>Якщо бази немає - створює її за моделлю і заповнює демо-даними.</summary>
        public static void EnsureCreated(string connectionString)
        {
            DatabaseInitializer.EnsureCreated(DatabaseInitializer.CreateOptions(connectionString));
        }
    }
}
