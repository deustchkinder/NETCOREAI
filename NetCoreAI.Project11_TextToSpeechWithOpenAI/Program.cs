using Newtonsoft.Json;
using System.Text;

class Program
{
    private static readonly string apiKey = "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";

    static async Task Main(string[] args)
    {
        Console.Write("Metni Giriniz: ");
        string input;
        input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Ses dosyası oluşuturuluyor....");
            await GenerateSpeech(input);
            Console.Write("Ses dosyası 'output mp3' olarak kaydedildi!");
            System.Diagnostics.Process.Start("explorer.exe", "output.mp3");

        }

        static async Task GenerateSpeech(string text)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestBody = new
                {
                    model = "tts-1",
                    input = text,
                    voice = "onyx"
                };

                string json = JsonConvert.SerializeObject(requestBody);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/audio/speech", content);

                if (response.IsSuccessStatusCode)
                {
                    byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync("output.mp3", audioBytes);
                }
                else
                {
                    Console.WriteLine("Bir hata oluştu");
                }

            }
        }
    }
}