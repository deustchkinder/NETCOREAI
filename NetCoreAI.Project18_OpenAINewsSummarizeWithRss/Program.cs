using System.Text;
using System.Text.Json;
using System.Xml.Linq;

class Program
{
    private static readonly string apiKey = "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";
    private static readonly string rssFeedUrl = "https://www.sozcu.com.tr/rss/tum-haberler.xml";

    static async Task Main()
    {
        Console.WriteLine("Haberler Sistemden Alınıyor...");
        List<string> articles = await FetchLatestNews(10);

        foreach (var article in articles)
        {
            Console.WriteLine("Haber özeti oluşturuluyor...");
            string summary = await SummarizeArticle(article);
            Console.WriteLine("--- AI tarafından özetlenen haber ---\n");
            Console.WriteLine(summary);
            Console.WriteLine("-------------------------------------------------\n");
        }

    }
    static async Task<List<string>> FetchLatestNews(int count)
    {
        var client = new HttpClient();
        string rssContent = await client.GetStringAsync(rssFeedUrl);

        XDocument doc = XDocument.Parse(rssContent);
        var items = doc.Descendants("item").Take(count);

        List<string> articles = items.Select(item =>
        {
            string title = item.Element("title")?.Value ?? "";
            string description = item.Element("description")?.Value ?? "";
            return $"{title}. {description}";
        }).ToList();

        return articles;
    }

    static async Task<string> SummarizeArticle(string articleText)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        var requestBody = new
        {
            model = "gpt-4-turbo",
            messages = new[]
            {
                    new { role = "system", content = "You are an expert news summarizer." },
                    new { role = "user", content = "Bu haberi 3 cümlede özetle: " + articleText }
                },
            max_tokens = 500
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);
        string responseContent = await response.Content.ReadAsStringAsync();

        JsonDocument doc = JsonDocument.Parse(responseContent);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

    }
}
