using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherbot.Providers
{
    internal class UserProvider
    {
        public static Models.User? GetUser(long tgid)
        {
            using (var db = new ApplicationContext())
            {
                return db.Users.Where(item => item.TgId == tgid).FirstOrDefault();
            }
        }

        public static void AddUser(Models.User user)
        {
            if (UserProvider.GetUser(user.TgId) == null)
            {
                using (var db = new ApplicationContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }

        public static List<Models.User> ListAllUsers()
        {
            using (var db = new ApplicationContext())
            {
                return db.Users.ToList();
            }
        }


        public static void ChangeState(long tgid, string state)
        {
            using (var db = new ApplicationContext())
            {
                var user =
                db.Users.Where(item => item.TgId == tgid).FirstOrDefault();
                user.State = state;
                db.SaveChanges();
            }
        }

        public static void ChangeCity(long tgid, string city)
        {
            using (var db = new ApplicationContext())
            {
                var user =
                    db.Users.Where(item => item.TgId == tgid).FirstOrDefault();
                if (user == null) return;
                user.City = city;
                user.State = "default";
                db.SaveChanges();
            }
        }

    }
}
