using Telegram.Bot;
using Telegram.Bot.Types;


namespace weatherbot
{
    internal class Management
    {
        static void Main(string[] args)
        {
            Host weatherbot = new Host("7814611141:AAGGDBntR2aPGqQ7AJcvxnzgPbMX-9JSaDk");
            weatherbot.Start();

            Console.ReadLine();


        }
    }
}