using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using weatherbot.Models;
using static System.Net.Mime.MediaTypeNames;

namespace weatherbot
{
    internal class Host
    {
        Stack<Chat> chats = new Stack<Chat>();
        TelegramBotClient _bot;
        private TimerCallback _timerCallback;
        private Timer _scheduleTimer;
        private static string city = "Moscow";
        public Uri uri = new Uri("http://api.openweathermap.org/data/2.5/find?q="+ city +"&type=like&APPID=de743d4490d7035d95832e6031995518");
        private WeatherAPI weatherAPI;
        private bool _isChange = false;

        public Host(string token)
        {
                _bot = new TelegramBotClient(token);
            _timerCallback = new TimerCallback(InvokeCallback);
            _scheduleTimer = new Timer(_timerCallback, 52, 0, 7000);
            weatherAPI = new WeatherAPI(uri);
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
            
            List<WeatherItem> items = weatherAPI.Get();
            

            if (update.Message?.Text == "/start")
            {
                chats.Push(update.Message.Chat);
                AddUser(new Models.User {TgId = update.Message.Chat.Id});
            }
            if (update.Message?.Text == "/send")
            {
                foreach (Chat chat in chats) 
                {
                    foreach (WeatherItem item in items)
                    {
                        
                    }
                }
            }
            if (update.Message?.Text == "/change")
            {
                //_bot.SendMessage(user.TgId, "penis");
                
                return;
            }

            Console.WriteLine($"got message: {update.Message?.Text ?? "[not text]"}");
            if (_isChange)
            {
                _isChange = false;
                city = update.Message?.Text;
                Console.WriteLine(city);
                uri = new Uri("http://api.openweathermap.org/data/2.5/find?q=" + city + "&type=like&APPID=de743d4490d7035d95832e6031995518");
                weatherAPI = new WeatherAPI(uri);
            }
            //return client.SendTextMessageAsync(update.Message?.Chat, "неформал");
            //ChatId chatId = 7330841099;
            //await _bot.SendMessage(update.Message.Chat, "bot is active");


        }



        private async Task SendMessages()
        {
            List<WeatherItem> items = weatherAPI.Get();
            List<Models.User> users = ListAllUsers();
            Console.WriteLine();
            foreach (Models.User user in users)
            {
                //_bot.SendMessage(user.TgId, "penis");
                var item = items[0];
                _bot.SendMessage(user.TgId, item.City + "\n" + item.Temp + "\n" + item.Humidity + "\n" + item.Description);
            }

        }

        public void AddUser(Models.User user)
        {
            if (GetUser(user.TgId) == null)
            {
                using (var db = new ApplicationContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }

        public Models.User? GetUser(long tgid)
        {
            using (var db = new ApplicationContext())
            {
                return db.Users.Where(item => item.TgId == tgid).FirstOrDefault();
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
