using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.Property(r => r.Name).IsRequired().HasMaxLength(ConfigurationConstants.NameLength);
            builder.HasOne(r => r.Country)
                .WithMany()
                .HasForeignKey(r => r.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(r => new { r.CountryId, r.Name }).IsUnique();
        }
    }
}
