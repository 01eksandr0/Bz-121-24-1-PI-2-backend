using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.Property(h => h.Name).IsRequired().HasMaxLength(ConfigurationConstants.NameLength);
            builder.Property(h => h.Description).HasMaxLength(ConfigurationConstants.DescriptionLength);
            builder.HasOne(h => h.Region)
                .WithMany()
                .HasForeignKey(h => h.RegionId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
