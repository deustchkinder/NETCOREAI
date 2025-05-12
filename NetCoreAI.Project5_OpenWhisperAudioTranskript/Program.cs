using System.Net.Http.Headers;


class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "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";
        string audioFilePath = "C:\\Users\\emred\\OneDrive\\Masaüstü\\.NET CORE\\Artificial Intelligence\\NetCoreAI\\NetCoreAI.Project5_OpenWhisperAudioTranskript\\mucevher.mp3";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var form = new MultipartFormDataContent();

            var audioContent = new ByteArrayContent(File.ReadAllBytes(audioFilePath));
            audioContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");
            form.Add(audioContent, "file", Path.GetFileName(audioFilePath));
            form.Add(new StringContent("whisper-1"), "model");

            Console.WriteLine("Ses Dosyası İşleniyor, Lütfen Bekleyiniz......");

            var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", form);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Transkript: ");
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"Hata: {response.StatusCode}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
/*
 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
 */