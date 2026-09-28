using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FutureVest.Persistence.EntityConfigurations
{
    public class RankingSimulationMacroindicatorEntityConfiguration : IEntityTypeConfiguration<RankingSimulationMacroindicator>
    {
        public void Configure(EntityTypeBuilder<RankingSimulationMacroindicator> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("RankingSimulationMacroindicators");
            #endregion

            #region Property configurations
            builder.Property(s => s.Weight).IsRequired().HasPrecision(5, 4);
            #endregion

            #region Index
            builder.HasIndex(s => s.MacroindicatorId).IsUnique();
            #endregion

            #region Relationships
            builder.HasOne(s => s.Macroindicator)
                   .WithMany()
                   .HasForeignKey(s => s.MacroindicatorId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
