using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Threading;
using System.Timers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using weatherbot.Models;
using weatherbot.Providers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using Timer = System.Threading.Timer;
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
            var user = UserProvider.GetUser(update.Message.Chat.Id  );
            /*if (update.Message?.Text == "/start")
            {
                UserProvider.AddUser(new Models.User { TgId = update.Message.Chat.Id });
                TimesProvider.AddTime(new Time { UserId = UserProvider.GetUser(update.Message.Chat.Id).Id });
            }

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
            }*/

            switch (update.Message?.Text)
            {
                case "/start":
                    UserProvider.AddUser(new Models.User { TgId = update.Message.Chat.Id});
                    TimesProvider.AddTime(new Time { UserId = UserProvider.GetUser(update.Message.Chat.Id).Id});
                    break;

                case "/change city":
                    await _bot.SendMessage(update.Message.Chat.Id, "Введите название города:");
                    UserProvider.ChangeState(update.Message.Chat.Id, "change city");
                    return;

                case "/change time":
                    var times = TimesProvider.Times(user.Id).Select(item => item.TimeStr);
                    string answer = "";
                    foreach (string time in times)
                    {
                        answer = answer + time + "\n";
                    }
                    await _bot.SendMessage(update.Message.Chat.Id, "Доступное время: " + "\n" + answer + "\n" +
                    "Выберите действие:" + "\n" + "1. Добавить время" + "\n" + "2. Удалить время");
                    UserProvider.ChangeState(update.Message.Chat.Id, "change time");
                    return;
            }
            if (user != null && user.State == "change city")
            {
                var city = update.Message?.Text;
                UserProvider.ChangeCity(update.Message.Chat.Id, city);
            }

            if (user != null && user.State == "change time")
            {
                if (update.Message.Text == "1")
                {
                    await _bot.SendMessage(update.Message.Chat.Id, "Введите время для отправки сообщений");
                    UserProvider.ChangeState(update.Message.Chat.Id, "add time");
                }
                else if (update.Message.Text == "2")
                {
                    await _bot.SendMessage(update.Message.Chat.Id, "Введите время, которое необходимо удалить");
                    UserProvider.ChangeState(update.Message.Chat.Id, "delete time");
                }
                else
                {
                    await _bot.SendMessage(update.Message.Chat.Id, "Введен некорректный формат для выполнения команды");
                    UserProvider.ChangeState(update.Message.Chat.Id, "default");
                }
            }

            if (user.State == "add time")
            {
                string time = update.Message.Text;
                TimesProvider.AddTime(new Time { UserId = UserProvider.GetUser(update.Message.Chat.Id).Id, TimeStr = time });
                UserProvider.ChangeState(update.Message.Chat.Id, "default");
            }
            
            if (user.State == "delete time")
            {
                string timestr = update.Message.Text;
                var time = TimesProvider.GetTime(timestr);
                TimesProvider.DeleteTime(time);
                UserProvider.ChangeState(update.Message.Chat.Id, "default");
            }

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
