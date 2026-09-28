using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FutureVest.Persistence.EntityConfigurations
{
    public class ReturnRateSettingEntityConfiguration : IEntityTypeConfiguration<ReturnRateSetting>
    {
        public void Configure(EntityTypeBuilder<ReturnRateSetting> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("ReturnRateSettings");
            #endregion

            #region Property configurations
            builder.Property(rrs => rrs.MinReturnRate).IsRequired().HasPrecision(4, 2);
            builder.Property(rrs => rrs.MaxReturnRate).IsRequired().HasPrecision(4, 2);
            #endregion
        }
    }
}
