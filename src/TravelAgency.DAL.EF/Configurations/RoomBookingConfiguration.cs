using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class RoomBookingConfiguration : IEntityTypeConfiguration<RoomBooking>
    {
        public void Configure(EntityTypeBuilder<RoomBooking> builder)
        {
            builder.Property(b => b.TotalPrice)
                .HasPrecision(ConfigurationConstants.MoneyPrecision, ConfigurationConstants.MoneyScale);
            builder.HasOne(b => b.Room)
                .WithMany()
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(b => new { b.RoomId, b.CheckIn });
        }
    }
}
