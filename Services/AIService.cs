namespace weatherbot.Services;

internal class AiService
{
    /// <summary>
    ///     Загружает из файла 'ai_context.txt' - в этом файле описывается паттерн ответа нейросети, а также
    ///     мы вводим нейросеть в курс дела, рассказываем что она должна делать, даем примеры.
    /// </summary>
    /// <returns></returns>
    private static string GetAiContext()
    {
        return File.ReadAllText("ai_context.txt");
    }

    public static async Task<string?> FormatWeatherByAi(string inputText)
    {
        try
        {
            var model = "gpt-4.1-mini";
            var pattern = GetAiContext();
            var aiAnswer = await AiApi.MakeRequest(model, pattern, inputText);

            if (aiAnswer == null)
            {
                Console.WriteLine("Ошибка обращения к ии-модели");
                return null;
            }

            return aiAnswer;
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetAiContext.FormatWeatherByAi - произошла ошибка:\n" +
                              ex.StackTrace);
            return null;
        }
    }
}

internal class AiRequestData
{
    /// <summary>
    ///     AcceptedStatus - отклонен или принят отзыв ИИ
    /// </summary>
    public bool AcceptedStatus { get; set; }

    /// <summary>
    ///     Пояснение ИИ
    /// </summary>
    public string Summary { get; set; }
}