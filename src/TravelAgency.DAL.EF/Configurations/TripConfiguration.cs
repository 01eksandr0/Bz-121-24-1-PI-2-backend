using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.Property(t => t.FromCity).IsRequired().HasMaxLength(ConfigurationConstants.NameLength);
            builder.Property(t => t.ToCity).IsRequired().HasMaxLength(ConfigurationConstants.NameLength);
            builder.Property(t => t.Price)
                .HasPrecision(ConfigurationConstants.MoneyPrecision, ConfigurationConstants.MoneyScale);
            builder.HasIndex(t => t.DepartureAt);
        }
    }
}
