using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class TourConfiguration : IEntityTypeConfiguration<Tour>
    {
        public void Configure(EntityTypeBuilder<Tour> builder)
        {
            builder.Property(t => t.Title).IsRequired().HasMaxLength(ConfigurationConstants.TitleLength);
            builder.Property(t => t.Description).HasMaxLength(ConfigurationConstants.DescriptionLength);
            builder.Property(t => t.PricePerPerson)
                .HasPrecision(ConfigurationConstants.MoneyPrecision, ConfigurationConstants.MoneyScale);
            builder.HasOne(t => t.Region)
                .WithMany()
                .HasForeignKey(t => t.RegionId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(t => t.StartDate);
        }
    }
}
