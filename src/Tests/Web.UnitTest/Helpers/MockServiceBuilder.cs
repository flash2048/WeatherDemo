using AutoMapper;
using AutoMapper.Internal;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Weather.Mapping;

namespace Web.UnitTest.Helpers
{
    internal static class MockServiceBuilder
    {
        private const string TestDbConnectionString = "Data Source=localhost;Initial Catalog=test;User ID=test;Password=test";
        public static IGlobalConfiguration GetMapperConfiguration()
        {
            static IEnumerable<Type> GetProfiles(params Type[] profileAssemblyMarkerTypes)
            {
                foreach (var profileAssemblyMarkerType in profileAssemblyMarkerTypes)
                    foreach (var type in profileAssemblyMarkerType.Assembly.GetTypes())
                        if (typeof(Profile).IsAssignableFrom(type))
                            yield return type;
            }

            return new MapperConfiguration(cfg =>
            {
                foreach (var profileType in GetProfiles(typeof(WebProfile)))
                    cfg.AddProfile(profileType);
            });
        }

        public static WeatherDbContext GetWeatherDbContextFake()
        {
            var dbOptions = new DbContextOptionsBuilder<WeatherDbContext>().UseSqlServer(TestDbConnectionString, e =>
                e.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
            return new WeatherDbContext(dbOptions.Options);
        }
        public static IMapper GetMapper()
        {
            return new Mapper(GetMapperConfiguration());
        }
    }
}
