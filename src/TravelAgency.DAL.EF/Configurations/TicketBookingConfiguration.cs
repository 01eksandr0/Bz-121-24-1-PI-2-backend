using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class TicketBookingConfiguration : IEntityTypeConfiguration<TicketBooking>
    {
        public void Configure(EntityTypeBuilder<TicketBooking> builder)
        {
            builder.Property(b => b.TotalPrice)
                .HasPrecision(ConfigurationConstants.MoneyPrecision, ConfigurationConstants.MoneyScale);
            builder.HasOne(b => b.Trip)
                .WithMany()
                .HasForeignKey(b => b.TripId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
