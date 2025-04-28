using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace weatherbot.Services
{
    internal class CitiesService
    {
        /// <summary>
        /// Верет нулл если еррор или террор
        /// </summary>
        /// <returns>аббаюдна</returns>
        public static List<City>? GetCities()
        {
            try
            {
                using (StreamReader streamReader = new StreamReader("cities.json"))
                {
                    /*string jsonContent = streamReader.ReadToEnd();
                    CitiesCollector? items = JsonConvert.DeserializeObject<CitiesCollector>(jsonContent);

                    return items?.Cities
                        .Select(item => new City { Name = item })
                        .ToList();*/
                    List<City> outputcities = new List<City>();
                    string json = streamReader.ReadToEnd();
                    CitiesCollector? items = JsonConvert.DeserializeObject<CitiesCollector>(json);
                    foreach (string city in items.Cities)
                    { 
                        City city1 = new City(city);
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
        public string Name { get; set; }
        public City(string name)
        {
            this.Name = name;
        }

    }
}
