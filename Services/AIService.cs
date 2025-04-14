using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace weatherbot.Services
{
    internal class AiService
    {
        /// <summary>
        /// Загружает из файла 'ai_context.txt' - в этом файле описывается паттерн ответа нейросети, а также 
        /// мы вводим нейросеть в курс дела, рассказываем что она должна делать, даем примеры.
        /// </summary>
        /// <returns></returns>
        private static string GetAiContext()
        {
            return File.ReadAllText("ai_context.txt");
        }

        private static readonly string AiAnswerPattern = "(Yes|No)\\ \\\"([^\\n]+)\\\"";
        public static async Task<AiRequestData?> CheckReviewByAi(string reviewText)
        {
            try
            {
                string model = "deepseek-3";
                string pattern = GetAiContext();
                string? aiAnswer = await AiApi.MakeRequest(model, pattern, reviewText);

                if (aiAnswer == null)
                {
                    Console.WriteLine("Ошибка обращения к ии-модели");
                    return null;
                }

                // вот пример как может выглядет aiAnswer:
                // Yes "Отзыв принят. Спасибо за ваше замечание!"

                // сперва нужно получить acceptedStatus
                var match = Regex.Match(aiAnswer, AiAnswerPattern);
                if (match == null || match.Groups.Count != 3)
                {
                    Console.WriteLine("Ошибка парсинга ответа ии-модели");
                    return null;
                }

                string state = match.Groups[1].Value;
                string summary = match.Groups[2].Value;

                return new AiRequestData
                {
                    AcceptedStatus = state.Equals("Yes"),
                    Summary = summary
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAiContext.CheckReviewByAi - произошла ошибка:\n" +
                    ex.StackTrace);
                return null;
            }
        }
    }

    internal class AiRequestData
    {
        /// <summary>
        /// AcceptedStatus - отклонен или принят отзыв ИИ
        /// </summary>
        public bool AcceptedStatus { get; set; }

        /// <summary>
        /// Пояснение ИИ
        /// </summary>
        public string Summary { get; set; }
    }
}