using Newtonsoft.Json.Linq;

namespace weatherbot.Services;

public class WeatherAPI
{
    public Uri endpoint;

    public WeatherAPI(Uri endpoint)
    {
        this.endpoint = endpoint;
    }

    public List<WeatherItem> Get()
    {
        var listOutputValue = new List<WeatherItem>();
        var client = new HttpClient();
        var result = client.GetAsync(endpoint).Result;
        var json = result.Content.ReadAsStringAsync().Result;
        var weatherData = JObject.Parse(json);


        foreach (JObject city in weatherData["list"])
        {
            var _city = $"Город: {city["name"]}";
            var temp = $"Температура: {(double)city["main"]["temp"] - 273.15:F1} °C";
            var humidity = $"Влажность: {city["main"]["humidity"]}%";
            var description = $"Описание погоды: {city["weather"][0]["description"]}";
            var weatherItem = new WeatherItem(_city, temp, humidity, description);
            listOutputValue.Add(weatherItem);
        }

        return listOutputValue;
    }
}

public class WeatherItem
{
    public WeatherItem(string City, string Temp, string Humidity, string Description)
    {
        this.City = City;
        this.Temp = Temp;
        this.Humidity = Humidity;
        this.Description = Description;
    }

    public string City { get; set; }
    public string Temp { get; set; }
    public string Humidity { get; set; }
    public string Description { get; set; }
}