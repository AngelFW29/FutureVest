using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FutureVest.Persistence.EntityConfigurations
{
    public class CountryIndicatorEntityConfiguration : IEntityTypeConfiguration<CountryIndicator>
    {
        public void Configure(EntityTypeBuilder<CountryIndicator> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("CountryIndicators");
            #endregion

            #region Property configurations
            builder.Property(ci => ci.Value).IsRequired().HasPrecision(18, 4);
            builder.Property(ci => ci.Year).IsRequired();
            #endregion

            #region Indexes
            builder.HasIndex(ci => new { ci.CountryId, ci.MacroindicatorId, ci.Year }).IsUnique();
            #endregion

            #region Relationships
            builder.HasOne(ci => ci.Country)
                .WithMany(c => c.Indicators)
                .HasForeignKey(ci => ci.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Macroindicator)
                   .WithMany(m => m.Indicators)
                   .HasForeignKey(ci => ci.MacroindicatorId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
