using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Web.UnitTest.Helpers;

namespace Web.UnitTest
{
    //TODO: better to use etalon files
    public class WeatherDbContextTest
    {
        [Fact]
        public void CheckGetForecastFromWeatherDbContext()
        {
            var query = MockServiceBuilder.GetWeatherDbContextFake()
                .Forecasts;

            query.ToQueryString().Should().Be("SELECT [f].[Date], [f].[MaxTemperature], [f].[MinTemperature]\r\nFROM [Forecasts] AS [f]");
        }

        [Fact]
        public void CheckGetForecastForSpecificDateFromWeatherDbContext()
        {
            var date = new DateOnly(2024, 4, 14);
            var query = MockServiceBuilder.GetWeatherDbContextFake()
                .Forecasts.Where(f => f.Date == date);

            query.ToQueryString().Should().Be("DECLARE @__date_0 date = '2024-04-14';\r\n\r\nSELECT [f].[Date], [f].[MaxTemperature], [f].[MinTemperature]\r\nFROM [Forecasts] AS [f]\r\nWHERE [f].[Date] = @__date_0");
        }
    }
}
