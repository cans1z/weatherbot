using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
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

        public static Time GetTime(string time)
        {
            using (var db = new ApplicationContext())
            {
                return db.Times.Where(item => item.TimeStr == time).FirstOrDefault();
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

        public static void DeleteTime(Time time)
        {
            using (var db = new ApplicationContext())
            {
                db.Times.Remove(time);
                db.SaveChanges();
            }
        }

        public static List<Time> ListAllTimes() 
        {
            using (var db = new ApplicationContext())
            {
                return db.Times.ToList();
            }
        }
    }
}
