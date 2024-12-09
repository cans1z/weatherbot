using Microsoft.VisualBasic;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;


namespace weatherbot
{
    internal class Host
    {
        public Host()
        {
            uri; 
        }
        Stack<Chat> chats = new Stack<Chat>();
        TelegramBotClient _bot;
        private TimerCallback _timerCallback;
        private Timer _scheduleTimer;
        public Uri uri = new Uri("http://api.openweathermap.org/data/2.5/find?q=Kirov&type=like&APPID=de743d4490d7035d95832e6031995518");

        public Host(string token)
        {
                _bot = new TelegramBotClient(token);
            _timerCallback = new TimerCallback(InvokeCallback);
            _scheduleTimer = new Timer(_timerCallback, 52, 0, 20000);
        }

        public void InvokeCallback(object obj)
        {
            SendMessage();
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
            WeatherAPI weather = new WeatherAPI();
            List<WeatherItem> items = weather.Get();
            

            if (update.Message?.Text == "/start")
            {
                chats.Push(update.Message.Chat);
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
                (string)uri.
            }
                Console.WriteLine($"got message: {update.Message?.Text ?? "[not text]"}");
            //return client.SendTextMessageAsync(update.Message?.Chat, "неформал");
            //ChatId chatId = 7330841099;
            //await _bot.SendMessage(update.Message.Chat, "bot is active");

        }

        private async Task SendMessage()
        {
            WeatherAPI weather = new WeatherAPI();
            List<WeatherItem> items = weather.Get();
            
            
            foreach (Chat chat in chats)
            {
                foreach (WeatherItem item in items)
                {
                    _bot.SendMessage(chat, item.City + "\n" + item.Temp + "\n" + item.Humidity + "\n" + item.Description);
                }
               
            }
        }




    }
}
