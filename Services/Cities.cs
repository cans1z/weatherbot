using Newtonsoft.Json;

namespace weatherbot.Services;

internal class CitiesService
{
    /// <summary>
    ///     Верет нулл если еррор или террор
    /// </summary>
    /// <returns>аббаюдна</returns>
    public static List<City>? GetCities()
    {
        try
        {
            using (var streamReader = new StreamReader("cities.json"))
            {
                /*string jsonContent = streamReader.ReadToEnd();
                CitiesCollector? items = JsonConvert.DeserializeObject<CitiesCollector>(jsonContent);

                return items?.Cities
                    .Select(item => new City { Name = item })
                    .ToList();*/
                var outputcities = new List<City>();
                var json = streamReader.ReadToEnd();
                var items = JsonConvert.DeserializeObject<CitiesCollector>(json);
                foreach (var city in items.Cities)
                {
                    var city1 = new City(city);
                    outputcities.Add(city1);
                }

                return outputcities;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}

internal class CitiesCollector
{
    public string[]? Cities { get; set; }
}

public class City
{
    public City(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}