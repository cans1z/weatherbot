using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using weatherbot.Models;
using weatherbot.Providers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using Update = Telegram.Bot.Types.Update;

namespace weatherbot
{
    internal class Host
    {
        TelegramBotClient _bot;
        private TimerCallback _timerCallback;
        private Timer _scheduleTimer;
        private WeatherAPI weatherAPI;
       

        public Host(string token)
        {
                _bot = new TelegramBotClient(token);
            _timerCallback = new TimerCallback(InvokeCallback);
            _scheduleTimer = new Timer(_timerCallback, 52, 0, 7000);
            //weatherAPI = new WeatherAPI(uri);
        }

        public void InvokeCallback(object obj)
        {
            
            SendMessages();
        }

        public void Start()
        {
            _bot.StartReceiving(UpdateHandler, ErrorHandler);
            Console.WriteLine("bot get started");
        }
                
       

        private Task ErrorHandler(ITelegramBotClient client, Exception exception,
            HandleErrorSource errorSource, CancellationToken cancellation)
        {
            Console.WriteLine("error" + exception.Message);
            return Task.CompletedTask;
        }

        public async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellation)
        {

            if (update.Message?.Text == "/start")
            {
                try
                {
                    AddUser(new Models.User { TgId = update.Message.Chat.Id });
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.StackTrace}");
                }
                
            }

            var user = UserProvider.GetUser(update.Message.Chat.Id);
            if (user != null && user.State == "change")
            {
                var city = update.Message?.Text;
                ChangeCity(update.Message.Chat.Id, city);
            }

            if (update.Message?.Text == "/change")
            {
                //_bot.SendMessage(user.TgId, "penis");

                await _bot.SendMessage(update.Message.Chat.Id, "Введите название города:");
                UserProvider.ChangeState(update.Message.Chat.Id, "change");
                return;
            }

            var user3 = UserProvider.GetUser(update.Message.Chat.Id);
            Console.WriteLine($"got message: {update.Message?.Text ?? "[not text]"}");
            Uri uri = new Uri("http://api.openweathermap.org/data/2.5/find?q=" + user3.City + "&type=like&APPID=de743d4490d7035d95832e6031995518");
            weatherAPI = new WeatherAPI(uri);
        }



        private async Task SendMessages()
        {
            List<Models.User> users = ListAllUsers();
            Console.WriteLine();
            foreach (Models.User user in users)
            {
                Uri uri = new Uri("http://api.openweathermap.org/data/2.5/find?q=" + user.City + "&type=like&APPID=de743d4490d7035d95832e6031995518");
                weatherAPI = new WeatherAPI(uri);
                List<WeatherItem> items = weatherAPI.Get();
                var item = items[0];
                _bot.SendMessage(user.TgId, item.City + "\n" + item.Temp + "\n" + item.Humidity + "\n" + item.Description);
            }
            
        }

        public void AddUser(Models.User user)
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

        public void ChangeCity(long tgid, string city)
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

        public List<Models.User> ListAllUsers()
        {
            using (var db = new ApplicationContext())
            {
                return db.Users.ToList();
            }
        }
    }
}
