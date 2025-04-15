using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherbot.Models;


namespace weatherbot.Providers
{
    class TimesProvider
    {
        public static List<Time> Times(int userid)
        {
            using (var db = new ApplicationContext())
            {
                return db.Times.Where(item => item.UserId == userid).ToList();
            }
        }

        public static void AddTime(Time time)
        {
            using (var db = new ApplicationContext())
            {
                db.Times.Add(time);
                db.SaveChanges();
            }
        }
    }
}
