﻿using System.Text;
using System.Text.Json;

namespace NetCoreAI.Project20_RecipeSuggestionWithOpenAI.Models
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;
        private const string OpenAiUrl = "https://api.openai.com/v1/chat/completions";
        private const string apiKey = "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";

        public OpenAiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public async Task<string> GetRecipeAsync(string ingredients)
        {
            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new {role="system",content="Sen profesyonel bir aşçısın. Kullanıcının elindeki malzemelere göre yemek tarifi öner."},
                    new {role="user",content=$"Elimde şu malzemeler var: {ingredients}. Ne yapabilirim?"}
                },
                temperature = 0.7
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var response = await _httpClient.PostAsync(OpenAiUrl, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(responseBody);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
    }
}
