﻿using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Polling;
using weatherbot.Models;
using weatherbot.Providers;
using weatherbot.Services;
using Timer = System.Threading.Timer;
using Update = Telegram.Bot.Types.Update;


namespace weatherbot;

internal class Management
{
    private readonly TelegramBotClient _bot;
    private Timer _scheduleTimer;
    private readonly TimerCallback _timerCallback;
    private WeatherAPI weatherAPI;


    public Management(string token)
    {
        _bot = new TelegramBotClient(token);
        _timerCallback = InvokeCallback;
        _scheduleTimer = new Timer(_timerCallback, 52, 0, 7000);
    }

    public List<City>? AvailableCities { get; private set; }

    public void InvokeCallback(object obj)
    {
        SendMessages();
    }

    public void Start()
    {
        _bot.StartReceiving(UpdateHandler, ErrorHandler);
        Console.WriteLine("bot get started");
        AvailableCities = CitiesService.GetCities();
        //Console.WriteLine(string.Join(",", items.Select(item => item.Name)));
    }

    private Task ErrorHandler(ITelegramBotClient client, Exception exception,
        HandleErrorSource errorSource, CancellationToken cancellation)
    {
        Console.WriteLine("error" + exception.Message);
        return Task.CompletedTask;
    }

    public async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellation)
    {
        if (update.Message == null)
        {
            Console.WriteLine("редачат пидорасы");
            return;
        }

        var user = UserProvider.GetUser(update.Message.Chat.Id);

        switch (update.Message?.Text)
        {
            case "/start":
                await _bot.SendMessage(update.Message.Chat.Id, "иди наухй короче погоду тебе в 7 утра покажут");

                if (user == null)
                    user = UserProvider.AddUser(new User { TgId = update.Message.Chat.Id });

                if (TimesProvider.Times(user.Id).Count == 0)
                    TimesProvider.AddTime(new Time { UserId = UserProvider.GetUser(update.Message.Chat.Id).Id });
                break;

            case "/change city":
                await _bot.SendMessage(update.Message.Chat.Id, "Введите название города:");
                UserProvider.ChangeState(update.Message.Chat.Id, "change city");
                return;

            case "/change time":
                var times = TimesProvider.Times(user.Id).Select(item => item.TimeStr);
                var answer = "";
                foreach (var time in times) answer = answer + time + "\n";
                await _bot.SendMessage(update.Message.Chat.Id, "Доступное время: \n" + answer + "\n" +
                                                               "Выберите действие: \n 1. Добавить время \n 2. Удалить время");
                UserProvider.ChangeState(update.Message.Chat.Id, "change time");
                return;
        }

        if (user != null && user.State == "change city")
        {
            var city = update.Message?.Text;
            /*if (AvailableCities.Where(item => item.Name == city).FirstOrDefault() == null)
            {
                await _bot.SendMessage(update.Message.Chat.Id, "еблан ты нет такого сити");
                return;
            }*/
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
            var _time = update.Message.Text;
            if (DateTime.TryParseExact(_time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
            {
                if (TimesProvider.Times(user.Id).Where(item => item.TimeStr == _time).FirstOrDefault() != null)
                {
                    Console.WriteLine("далбаеб ты");
                    await _bot.SendMessage(update.Message.Chat.Id, "че ты умный самый");
                    return;
                }

                await _bot.SendMessage(update.Message.Chat.Id, "Время было успешно добавлено");
                TimesProvider.AddTime(new Time
                    { UserId = UserProvider.GetUser(update.Message.Chat.Id).Id, TimeStr = _time });
                UserProvider.ChangeState(update.Message.Chat.Id, "default");
            }
            else
            {
                await _bot.SendMessage(update.Message.Chat.Id,
                    "Неверный формат времени. Пожалуйста, введите в формате HH:mm (например, 22:34).");
            }
        }

        if (user.State == "delete time")
        {
            var timestr = update.Message.Text;
            var time = TimesProvider.GetTime(timestr);
            if (time != null)
            {
                TimesProvider.DeleteTime(time);
                await _bot.SendMessage(update.Message.Chat.Id, "Время было успешно удалено");
                UserProvider.ChangeState(update.Message.Chat.Id, "default");
            }
            else
            {
                await _bot.SendMessage(update.Message.Chat.Id,
                    "Вы ввели несуществующее время, введите время из списка выше");
            }
        }
    }

    private async Task SendMessages()
    {
        var users = UserProvider.ListAllUsers();
        Console.WriteLine();
        /*Stream xJui = buster.startStream({tyan: true, twitch: true, yt: true, casino: true});*/
        var now = DateTime.Now; // 11:07
        var currentTime = $"{now:t}".Split(' ')[0];

        foreach (var user in users)
        {
            if (user.State != "default") continue;

            var times = TimesProvider.Times(user.Id).Select(item => item.TimeStr); // 4:10, 10:00
            if (!times.Contains(currentTime)) continue;

            var uri = new Uri("http://api.openweathermap.org/data/2.5/find?q=" + user.City +
                              "&type=like&APPID=de743d4490d7035d95832e6031995518");
            weatherAPI = new WeatherAPI(uri);
            var items = weatherAPI.Get();
            var item = items[0];
            //_bot.SendMessage(user.TgId, item.City + "\n" + item.Temp + "\n" + item.Humidity + "\n" + item.Description);
            _bot.SendMessage(user.TgId,
                await AiService.FormatWeatherByAi(item.City + "\n" + item.Temp + "\n" + item.Humidity + "\n" +
                                                  item.Description));
        }
    }
}