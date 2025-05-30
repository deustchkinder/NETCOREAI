﻿using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OpenAI.Chat;
using System.Threading.Tasks;


class Program
{

    static async Task Main(string[] args)
    {
        var apiKey = "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";
        Console.WriteLine("Lütfen sorunuzu yazınız: ('Mustafa Kemal Atatürk Kimdir?')");

        var prompt = Console.ReadLine();
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new {role="system",content="You are a helpful assistant."},
                new {role="user",content=prompt}
            },
            max_tokens = 500
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<JsonElement>(responseString);
                var answer = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                Console.WriteLine("Open AI'nin Cevabı: ");
                Console.WriteLine(answer);
            }
            else
            {
                Console.WriteLine($"Bir hata oluştu: {response.StatusCode}");
                Console.WriteLine(responseString);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Bir hata oluştu: {ex.Message}");
        }
    }
}


/*
  Authorization Bearer 123456abcde

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

  */