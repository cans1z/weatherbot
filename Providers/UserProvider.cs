using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherbot.Models;

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

        public static User? AddUser(User user)
        {
            using (var db = new ApplicationContext())
            {
                var newuser = db.Users.Add(user).Entity;
                db.SaveChanges();
                return newuser;
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
