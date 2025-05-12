using Newtonsoft.Json;
using System.Text;

class Program
{
    public static async Task Main(string[] args)
    {
        string apiKey = "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";
        Console.Write("Çizilmesini istediğiniz içerik (prompt giriniz...): ");
        string prompt;
        prompt = Console.ReadLine();
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var requestBody = new
            {
                prompt = prompt,
                n = 1,
                size = "1024x1024"
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/images/generations", content);
            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }
    }
}
//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