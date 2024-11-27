using System;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;

namespace weatherbot
{
    internal class Host
    {

        TelegramBotClient _bot;
        public Host(string token)
        {
            _bot = new TelegramBotClient(token);
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
            Console.WriteLine($"got message: {update.Message?.Text ?? "[not text]"}");
            //return client.SendTextMessageAsync(update.Message?.Chat, "неформал");
            //await _bot.SendMessage(chatId, "Welcome! Pick one direction");
            ChatId chatId = 7330841099;
            await _bot.SendMessage(chatId, "Trying all the parameters of sendMessage method");

        }


    }
}
