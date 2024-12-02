using Microsoft.VisualBasic;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Quartz;
using static System.Net.Mime.MediaTypeNames;

namespace weatherbot
{
    internal class Host
    {

        Stack<Chat> chats = new Stack<Chat>();
        TelegramBotClient _bot;
        private TimerCallback _timerCallback;
        private Timer _scheduleTimer;

        public Host(string token)
        {
            _bot = new TelegramBotClient(token);
            _timerCallback = new TimerCallback(InvokeCallback);
            _scheduleTimer = new Timer(_timerCallback, 52, 0, 1000);
        }

        public void InvokeCallback(object obj)
        {

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

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellation)
        {
            if (update.Message?.Text == "/start")
            {
                chats.Push(update.Message.Chat);
            }
            if (update.Message?.Text == "/send")
            {
                foreach (Chat chat in chats) 
                {
                    _bot.SendMessage(chat, "gay");
                }
            }
            Console.WriteLine($"got message: {update.Message?.Text ?? "[not text]"}");
            //return client.SendTextMessageAsync(update.Message?.Chat, "неформал");
            //ChatId chatId = 7330841099;
            await _bot.SendMessage(update.Message.Chat, "bot is active");

        }

        private async Task SendMessage()
        {
            foreach (Chat chat in chats)
            {
                _bot.SendMessage(chat, "gay");
            }
        }



        //public void StartScheduling(DateTime scheduledTime)
        //{   

        //}




    }
}
