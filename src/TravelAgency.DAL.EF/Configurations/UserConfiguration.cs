using TravelAgency.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAgency.DAL.EF.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FullName).IsRequired().HasMaxLength(ConfigurationConstants.NameLength);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(ConfigurationConstants.EmailLength);
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(ConfigurationConstants.PasswordHashLength);
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
