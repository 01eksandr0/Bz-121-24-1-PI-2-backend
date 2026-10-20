using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasMaxLength(ConfigurationConstants.NameLength);
            builder.HasIndex(c => c.Name).IsUnique();
        }
    }
}
