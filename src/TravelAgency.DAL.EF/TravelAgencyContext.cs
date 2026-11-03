using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace TravelAgency.DAL.EF
{
    /// <summary>Контекст EF Core (Code First). Налаштування сутностей - у папці Configurations.</summary>
    public class TravelAgencyContext : DbContext
    {
        public TravelAgencyContext(DbContextOptions<TravelAgencyContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Tour> Tours { get; set; }

        public DbSet<TourBooking> TourBookings { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<RoomBooking> RoomBookings { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<TicketBooking> TicketBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TravelAgencyContext).Assembly);
        }
    }
}
