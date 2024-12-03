using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;

namespace weatherbot
{
    static class WeatherAPI
    {
        public static void Get()
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri("http://api.openweathermap.org/data/2.5/find?q=Kirov&type=like&APPID=de743d4490d7035d95832e6031995518");
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(json.GetType());
                var city = client.GetFromJsonAsync<>("http://api.openweathermap.org/data/2.5/find?q=Kirov&type=like&APPID=de743d4490d7035d95832e6031995518")
            }

        }
    }
}
