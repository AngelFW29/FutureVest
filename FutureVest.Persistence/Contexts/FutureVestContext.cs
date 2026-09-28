using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace FutureVest.Persistence.Contexts
{
    public class FutureVestContext : DbContext
    {
        public FutureVestContext(DbContextOptions<FutureVestContext> options) : base(options) { }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Macroindicator> Macroindicators { get; set; }
        public DbSet<CountryIndicator> CountryIndicators { get; set; }
        public DbSet<ReturnRateSetting> ReturnRateSettings { get; set; }
        public DbSet<RankingSimulationMacroindicator> RankingSimulationMacroindicators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
