using Microsoft.Extensions.Configuration;
namespace Infrastructure.Helpers
{
    public static class ConnectionStringsHelper
    {
        public static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true) //TODO: move to constants
                .Build();
        }
    }
}
