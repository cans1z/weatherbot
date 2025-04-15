using Telegram.Bot;
using Telegram.Bot.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static weatherbot.Models.User;


namespace weatherbot
{
    internal class Host
    {
        static void Main(string[] args)
        {
            Management weatherbot = new Management("7814611141:AAGGDBntR2aPGqQ7AJcvxnzgPbMX-9JSaDk");
            //weatherbot.AddUser(new Models.User { TgId = 1231321312});
            weatherbot.Start();
            Console.ReadLine();
        }
    }
}