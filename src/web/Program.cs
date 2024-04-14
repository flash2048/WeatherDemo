using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Weather.Mapping;

namespace Weather
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<WeatherDbContext>(options =>
                               options.UseSqlServer(builder.Configuration.GetConnectionString("WeatherContext"),
                                   x => x.MigrationsAssembly("Weather")));

            builder.Services.AddScoped<IWeatherDbContext>(e => e.GetRequiredService<WeatherDbContext>());

            builder.Services.AddAutoMapper(_ =>
            {
                // cfg.AllowNullCollections = true;
            }, typeof(WebProfile)); // Scan current and infrastructure assemblies


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
                {
                    c.MapType<DateOnly>(() => new OpenApiSchema
                    {
                        Type = "string",
                        Format = "date"
                    });
                    // using System.Reflection;
                    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                }
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
