using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace weatherbot
{
    internal class Cities
    {
        public static List<City> Get()
        {
            using (StreamReader r = new StreamReader(@"C:\Users\personal pc\Documents\GitHub\weatherbot\cities.json"))
            {
                List<City> listOutputValue = new List<City>();
                string json = r.ReadToEnd();
                List<City> items = JsonConvert.DeserializeObject<List<City>>(json);
                foreach (City city in items)
                 {
                     string _city = "${city[\"name\"]}";
                     listOutputValue.Add(city);
                 }
                 return listOutputValue;
                return items;
            }
        }

        public class City
        {
            public string Name { get; set; }
        }
    }
}
