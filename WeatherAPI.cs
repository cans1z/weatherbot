using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace weatherbot
{
    public class WeatherAPI
    {
        public Uri endpoint;
        public WeatherAPI(Uri endpoint)
        {
            this.endpoint = endpoint;
        }

        public List<WeatherItem> Get()
        {
            List<WeatherItem> listOutputValue = new List<WeatherItem>();
            var client = new HttpClient();
            var result = client.GetAsync(endpoint).Result;
            var json = result.Content.ReadAsStringAsync().Result;
            JObject weatherData = JObject.Parse(json);
                

            foreach (JObject city in weatherData["list"])
            {
                string _city = $"Город: {city["name"]}";
                string temp = $"Температура: {(double)city["main"]["temp"] - 273.15:F1} °C";
                string humidity = $"Влажность: {city["main"]["humidity"]}%"; 
                string description = $"Описание погоды: {city["weather"][0]["description"]}";
                //Console.WriteLine(_city);
                //Console.WriteLine(temp);
                //Console.WriteLine(humidity);
                //Console.WriteLine(description);
                //Console.WriteLine("--------------------");

                WeatherItem weatherItem = new WeatherItem(_city, temp, humidity, description);
                listOutputValue.Add(weatherItem);
            }
            return listOutputValue;
        }
    }

    public class WeatherItem
    {
        public string City { get; set; }
        public string Temp { get; set; }    
        public string Humidity { get; set; }    
        public string Description { get; set; }

        public WeatherItem(string City, string Temp, string Humidity, string Description) 
        {
            this.City = City;   
            this.Temp = Temp;
            this.Humidity = Humidity;
            this.Description = Description;
        }

    }
}
