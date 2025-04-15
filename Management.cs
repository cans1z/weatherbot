using Microsoft.AspNetCore.Localization;
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
    internal class Management
    {
        TelegramBotClient _bot;
        private TimerCallback _timerCallback;
        private Timer _scheduleTimer;
        private WeatherAPI weatherAPI;
       

        public Management(string token)
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
                UserProvider.AddUser(new Models.User { TgId = update.Message.Chat.Id });
                TimesProvider.AddTime(new Time { UserId = UserProvider.GetUser(update.Message.Chat.Id).Id });
            }

            var user = UserProvider.GetUser(update.Message.Chat.Id);
            if (user != null && user.State == "change city")
            {
                var city = update.Message?.Text;
                UserProvider.ChangeCity(update.Message.Chat.Id, city);
            }

            if (update.Message?.Text == "/change city")
            {
                await _bot.SendMessage(update.Message.Chat.Id, "Введите название города:");
                UserProvider.ChangeState(update.Message.Chat.Id, "change city");
                return;
            }

            if (update.Message?.Text == "/change time")
            {
                await _bot.SendMessage(update.Message.Chat.Id, "Введите время для отправки сообщений:");
                UserProvider.ChangeState(update.Message.Chat.Id, "change time");
                return;
            }

            var user2 = UserProvider.GetUser(update.Message.Chat.Id);
            Console.WriteLine($"got message: {update.Message?.Text ?? "[not text]"}");
            Uri uri = new Uri("http://api.openweathermap.org/data/2.5/find?q=" + user2.City + "&type=like&APPID=de743d4490d7035d95832e6031995518");
            weatherAPI = new WeatherAPI(uri);
        }



        private async Task SendMessages()
        {
            List<Models.User> users = UserProvider.ListAllUsers();
            Console.WriteLine();
            DateTime now = DateTime.Now; // 11:07
            string currentTime = $"{now:t}".Split(' ')[0];

            foreach (Models.User user in users)
            {
                if (user.State != "default") continue;

                var times = TimesProvider.Times(user.Id).Select(item => item.TimeStr); // 4:10, 10:00
                if (!times.Contains(currentTime)) continue;

                Uri uri = new Uri("http://api.openweathermap.org/data/2.5/find?q=" + user.City + "&type=like&APPID=de743d4490d7035d95832e6031995518");
                weatherAPI = new WeatherAPI(uri);
                List<WeatherItem> items = weatherAPI.Get();
                var item = items[0];
                _bot.SendMessage(user.TgId, item.City + "\n" + item.Temp + "\n" + item.Humidity + "\n" + item.Description);
            }
            
        }
    }
}
