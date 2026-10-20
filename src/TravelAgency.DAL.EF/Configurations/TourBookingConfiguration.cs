using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class TourBookingConfiguration : IEntityTypeConfiguration<TourBooking>
    {
        public void Configure(EntityTypeBuilder<TourBooking> builder)
        {
            builder.Property(b => b.TotalPrice)
                .HasPrecision(ConfigurationConstants.MoneyPrecision, ConfigurationConstants.MoneyScale);
            builder.HasOne(b => b.Tour)
                .WithMany()
                .HasForeignKey(b => b.TourId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
