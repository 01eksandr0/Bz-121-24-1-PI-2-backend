using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(r => r.Name).IsRequired().HasMaxLength(ConfigurationConstants.NameLength);
            builder.Property(r => r.PricePerNight)
                .HasPrecision(ConfigurationConstants.MoneyPrecision, ConfigurationConstants.MoneyScale);
        }
    }
}
