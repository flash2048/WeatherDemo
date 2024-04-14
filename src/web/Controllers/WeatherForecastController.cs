using AutoMapper;
using Infrastructure.EF;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Weather.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get forecast for today
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ForecastView>> GetTodayForecastAsync()
        {
            var now = DateOnly.FromDateTime(DateTime.Now);

            var forecastEntity = await _context.Forecasts.AsNoTracking()
                                    .FirstOrDefaultAsync(e => e.Date == now);
            if (forecastEntity == null)
                return NotFound("Forecast for today not found");
            return _mapper.Map<ForecastView>(forecastEntity);
        }

        /// <summary>
        /// Get forecast for a specific date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("foraday")]
        public async Task<ActionResult<ForecastView>> GetForecastForADayAsync(DateOnly date)
        {
            var forecastEntity = await _context.Forecasts.AsNoTracking()
                                    .FirstOrDefaultAsync(e => e.Date == date);
            if (forecastEntity == null)
                return NotFound($"Forecast for date '{date}' not found");

            return _mapper.Map<ForecastView>(forecastEntity);
        }

        /// <summary>
        /// Get forecast for a week
        /// </summary>
        /// <returns></returns>
        [HttpGet("foraweek")]
        public async Task<IEnumerable<ForecastView>> GetForecastForAWeekAsync()
        {
            var now = DateOnly.FromDateTime(DateTime.Now);

            var forecastForAWeek = (await _context.Forecasts.AsNoTracking()
                                                        .Where(e => e.Date >= now && e.Date <= now.AddDays(7)).ToListAsync())
                                                        .Select(f => _mapper.Map<ForecastView>(f));

            return forecastForAWeek;
        }

        /// <summary>
        /// Delete forecast for a specific date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteForecastForADayAsync(DateOnly date)
        {
            var forecastEntity = await _context.Forecasts
                .FirstOrDefaultAsync(e => e.Date == date);
            if (forecastEntity == null)
                return NotFound($"Forecast for date '{date}' not found");

            _context.Forecasts.Remove(forecastEntity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Set forecast for a specific date
        /// </summary>
        /// <param name="forecast"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ForecastView>> SetForecastForADayAsync([FromBody] Forecast forecast)
        {
            if (forecast.MinTemperature > forecast.MaxTemperature)
                return BadRequest("Min temperature cannot be greater than max temperature");

            var now = DateOnly.FromDateTime(DateTime.Now);
            if (now > forecast.Date)
                return BadRequest("Cannot set forecast for past dates");

            var forecastEntity = await _context.Forecasts.FirstOrDefaultAsync(e => e.Date == forecast.Date);
            if (forecastEntity == null)
            {
                var newForecast = _mapper.Map<ForecastEntity>(forecast);
                _context.Forecasts.Add(newForecast);
                _logger.LogInformation("Forecast for {date} added", forecast.Date);
            }
            else
            {
                _mapper.Map(forecast, forecastEntity);
                _logger.LogInformation("Forecast for {date} updated", forecast.Date);
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<ForecastView>(forecastEntity);
        }
    }
}
