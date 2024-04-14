using Infrastructure.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public interface IWeatherDbContext
{
    DbSet<ForecastEntity> Forecasts { get; set; }
}