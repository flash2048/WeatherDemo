using Infrastructure.EF.Entities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.EF
{
    public class WeatherDbContext : DbContext, IWeatherDbContext
    {
        public DbSet<ForecastEntity> Forecasts { get; set; }

#pragma warning disable CS8618
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
#pragma warning restore CS8618
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConnectionStringsHelper.BuildConfiguration().GetConnectionString("WeatherContext");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        #region Model building
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ForecastEntity>(typeBuilder =>
            {
                typeBuilder.HasKey(e => e.Date);

            });
        }
        #endregion
    }

    public class WeatherDesignTimeDbContextFactory : IDesignTimeDbContextFactory<WeatherDbContext>
    {
        public WeatherDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();

            var connectionString = ConnectionStringsHelper.BuildConfiguration().GetConnectionString("WeatherContext");
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly("Weather"));

            return new WeatherDbContext(optionsBuilder.Options);
        }
    }
}
