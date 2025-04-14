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

    }
}
