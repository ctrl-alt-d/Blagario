using System;

namespace blagario.Data
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public long TemperatureC { get; set; }

        public long TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
