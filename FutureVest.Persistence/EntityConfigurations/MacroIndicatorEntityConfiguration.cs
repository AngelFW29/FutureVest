using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FutureVest.Persistence.EntityConfigurations
{
    public class MacroIndicatorEntityConfiguration : IEntityTypeConfiguration<Macroindicator>
    {
        public void Configure(EntityTypeBuilder<Macroindicator> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Macroindicators");
            #endregion

            #region Property configurations
            builder.Property(m => m.Name).IsRequired().HasMaxLength(20);
            builder.Property(m => m.Weight).IsRequired().HasPrecision(5, 4);
            #endregion
        }
    }
}