using AutoMapper;
using FluentAssertions;
using Infrastructure.EF;
using Infrastructure.EF.Entities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using Weather.Controllers;
using Web.UnitTest.Helpers;

namespace Web.UnitTest
{
    public class WeatherForecastControllerTests
    {
        private readonly Mock<ILogger<WeatherForecastController>> _mockLogger = new();
        private readonly IMapper _mockMapper = MockServiceBuilder.GetMapper();

        private readonly ServiceProvider _serviceProvider;

        public WeatherForecastControllerTests()
        {
            var services = new ServiceCollection();

            // Using In-Memory database for testing
            services.AddDbContext<WeatherDbContext>(options =>
                options.UseInMemoryDatabase("Weather"));

            _serviceProvider = services.BuildServiceProvider();
        }

        // Test GetForecastForADayAsync
        // Test GetTodayForecastAsync
        [Fact]
        public async Task GetForecastTestAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<WeatherDbContext>();
            dbContext.Forecasts.RemoveRange(await dbContext.Forecasts.ToListAsync());
            await dbContext.SaveChangesAsync();

            var now = DateOnly.FromDateTime(DateTime.Now);
            var forecast = new ForecastEntity
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                MinTemperature = 25,
                MaxTemperature = 30
            };

            await dbContext.Forecasts.AddAsync(forecast);
            await dbContext.SaveChangesAsync();

            var controller = new WeatherForecastController(_mockLogger.Object, dbContext, _mockMapper);

            // get forecast for today by specific date
            var result = await controller.GetForecastForADayAsync(now);
            result.Value.Should().NotBeNull();
            result.Value!.Date.Should().Be(now);
            result.Value!.Description.Should()
                .Be(ForecastConverter.ConvertTempToDescription((forecast.MinTemperature + forecast.MaxTemperature) /
                                                               2));
            // get forecast for today
            result = await controller.GetTodayForecastAsync();
            result.Value.Should().NotBeNull();
            result.Value!.Date.Should().Be(now);
            result.Value!.Description.Should()
                .Be(ForecastConverter.ConvertTempToDescription((forecast.MinTemperature + forecast.MaxTemperature) /
                                                               2));
        }

        // Test GetForecastForAWeekAsync
        // Test DeleteForecastForADayAsync
        // Test SetForecastForADayAsync
        [Fact]
        public async Task GetForecastForAWeekTestAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<WeatherDbContext>();
            dbContext.Forecasts.RemoveRange(await dbContext.Forecasts.ToListAsync());
            await dbContext.SaveChangesAsync();

            var now = DateOnly.FromDateTime(DateTime.Now);
            var list = new List<ForecastEntity>();
            for (var i = 0; i < 7; i++)
            {
                var rnd = new Random();
                var forecast = new ForecastEntity
                {
                    Date = now.AddDays(i),
                    MinTemperature = (sbyte)rnd.Next(-60, 60),
                    MaxTemperature = (sbyte)rnd.Next(-60, 60),
                };
                if (forecast.MinTemperature > forecast.MaxTemperature)
                {
                    (forecast.MinTemperature, forecast.MaxTemperature) = (forecast.MaxTemperature, forecast.MinTemperature);
                }
                list.Add(forecast);
            }

            await dbContext.Forecasts.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            var controller = new WeatherForecastController(_mockLogger.Object, dbContext, _mockMapper);

            // get forecast for a week
            var result = (await controller.GetForecastForAWeekAsync()).ToArray();
            result.Length.Should().Be(7);

            for (var i = 0; i < 7; i++)
            {
                result[i].Date.Should().Be(now.AddDays(i));
                result[i].Description.Should().Be(ForecastConverter.ConvertTempToDescription((list[i].MinTemperature + list[i].MaxTemperature) / 2));
            }

            // delete forecast for a day (today)
            await controller.DeleteForecastForADayAsync(now);
            result = (await controller.GetForecastForAWeekAsync()).ToArray();
            result.Length.Should().Be(6);

            await controller.SetForecastForADayAsync(new Forecast() { Date = now, MaxTemperature = 40, MinTemperature = 20 });

            result = (await controller.GetForecastForAWeekAsync()).ToArray();
            result.Length.Should().Be(7);
            result[0].Date.Should().Be(now);
            result[0].Description.Should().Be(ForecastConverter.ConvertTempToDescription(30));
        }
    }
}
