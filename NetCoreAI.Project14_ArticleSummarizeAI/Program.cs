﻿using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Program
{
    private static readonly string apiKey = "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";
    static async Task Main(string[] args)
    {
        Console.Write("Uzun metninizi veya makalenizi giriniz: ");
        string input;
        input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine();
            Console.WriteLine("Girmiş olduğunuz metin AI tarafından özetleniyor...");
            Console.WriteLine();

            string shortSummary = await SummarizeText(input, "short");
            string mediumSummary = await SummarizeText(input, "medium");
            string detailedSummary = await SummarizeText(input, "detailed");

            Console.WriteLine("Özetler");
            Console.WriteLine("------------------------");
            Console.WriteLine($" ** Kısa Özet: ** {shortSummary}");
            Console.WriteLine("------------------------");
            Console.WriteLine($" ** Orta Uzunlukta Özet: ** {mediumSummary}");
            Console.WriteLine("------------------------");
            Console.WriteLine($" ** Detaylı Özet: ** {detailedSummary}");
        }



        async Task<string> SummarizeText(string text, string level)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                string instruction = level switch
                {
                    "short" => "Summarize this text in 1-2 sentences.",
                    "medium" => "Summarize this text in 3-5 sentences.",
                    "detailed" => "Summarize this text in a detailed but concise manner.",
                    _ => "Summerize this text."
                };

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                    new {role="system",content="You are an AI that summarize text info different leves: short, medium and detailed."},
                    new {role="user",content=$"{instruction}\n\n{text}"}
                }
                };

                string json = JsonConvert.SerializeObject(requestBody);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                    return result.choices[0].message.content.ToString();
                }
                else
                {
                    Console.WriteLine($"Hata: {responseJson}");
                    return "Hata!";
                }
            }
        }
    }
}