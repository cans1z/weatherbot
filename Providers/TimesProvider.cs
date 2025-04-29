using weatherbot.Models;

namespace weatherbot.Providers;

internal class TimesProvider
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