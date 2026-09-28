using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FutureVest.Persistence.EntityConfigurations
{
    public class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Countries");
            #endregion

            #region Property & index configurations
            builder.Property(c => c.Name).IsRequired().HasMaxLength(60);
            builder.Property(c => c.IsoCode).IsRequired().HasMaxLength(2);
            builder.HasIndex(c => c.IsoCode).IsUnique();
            #endregion
        }
    }
}
