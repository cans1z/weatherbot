using System.Text;
using System.Text.Json;

namespace weatherbot.Services;

internal class Message
{
    public string Role { get; set; }
    public string Content { get; set; }
}

internal class RequestData
{
    public string Model { get; set; }
    public Message[] Messages { get; set; }
}

/// <summary>
///     Реализует апи для работы с моделью нейросети
/// </summary>
internal class AiApi
{
    /// <summary>
    ///     Делает http-запрос к АИ - модели
    /// </summary>
    /// <param name="modelName">Название модели, напр: "deepseek-3"</param>
    /// <param name="pattern">Паттерн для нейросити (рассказываем что она должна делать)</param>
    /// <param name="messageContent">Сообщение нейросети</param>
    /// <returns>Ответ нейросети</returns>
    public static async Task<string?> MakeRequest(string modelName, string pattern, string messageContent)
    {
        var token = "shds-chuvXBR1SmgS3YO2l7pDAKoyxz7";
        var uri = "https://gptunnel.ru/v1/chat/completions";

        using (var client = new HttpClient())
        {
            var requestData = new RequestData
            {
                Model = modelName,
                Messages = new[]
                {
                    new Message { Role = "system", Content = pattern },
                    new Message { Role = "user", Content = messageContent }
                }
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // пример: Model -> model, Role -> role
            };

            var json = JsonSerializer.Serialize(requestData, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", token);

            try
            {
                var response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                using (var responseJson = JsonDocument.Parse(responseBody))
                {
                    if (!responseJson.RootElement.TryGetProperty("choices", out var choices) ||
                        choices.GetArrayLength() <= 0) return null;

                    if (!choices[0].TryGetProperty("message", out var message) ||
                        !message.TryGetProperty("content", out var contentElement)) return null;

                    return contentElement.GetString();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"AiApi.MakeRequest: {e.StackTrace}");
                return null;
            }
        }
    }
}