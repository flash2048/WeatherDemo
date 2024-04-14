namespace Infrastructure.Helpers
{
    public static class ForecastConverter
    {
        public static string ConvertTempToDescription(int temperature)
        {
            if (temperature <= -20)
                return "Freezing";
            if (temperature <= -10)
                return "Bracing";
            if (temperature <= 0)
                return "Chilly";
            if (temperature <= 10)
                return "Cool";
            if (temperature <= 20)
                return "Mild";
            if (temperature <= 30)
                return "Warm";
            if (temperature <= 35)
                return "Balmy";
            if (temperature <= 40)
                return "Hot";
            if (temperature <= 45)
                return "Sweltering";
            else
                return "Scorching";
        }
    }
}
