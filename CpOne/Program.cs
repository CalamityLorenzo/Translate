using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CpOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var key = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzY29wZSI6Imh0dHBzOi8vYXBpLm1pY3Jvc29mdHRyYW5zbGF0b3IuY29tLyIsInN1YnNjcmlwdGlvbi1pZCI6IjUxZWY0Y2U2NDMxNDQ1NGU4NjRmNjliNjUwN2VlMDc4IiwicHJvZHVjdC1pZCI6IlRleHRUcmFuc2xhdG9yLlMxIiwiY29nbml0aXZlLXNlcnZpY2VzLWVuZHBvaW50IjoiaHR0cHM6Ly9hcGkuY29nbml0aXZlLm1pY3Jvc29mdC5jb20vaW50ZXJuYWwvdjEuMC8iLCJhenVyZS1yZXNvdXJjZS1pZCI6Ii9zdWJzY3JpcHRpb25zL2FjMzcyZGU4LTI1NDktNDdjYy1hYmUzLTg5ZDMxN2U5ZmNlMy9yZXNvdXJjZUdyb3Vwcy90cmFuc2xhdGVyZXNvdXJjZS9wcm92aWRlcnMvTWljcm9zb2Z0LkNvZ25pdGl2ZVNlcnZpY2VzL2FjY291bnRzL3hwY2xUcmFuc2xhdGUiLCJpc3MiOiJ1cm46bXMuY29nbml0aXZlc2VydmljZXMiLCJhdWQiOiJ1cm46bXMubWljcm9zb2Z0dHJhbnNsYXRvciIsImV4cCI6MTQ5MTU1NjEzM30.vnmljBp4RbPLij19Yk6UkiPMv1V71GehvabP4K9hr78";
            if (args.Length > 0)
            {
                key = args[0];
            }

            DoThing().Wait();



        }

        static async Task<String> ReadFile(string fileName)
        {
            using (var sr = File.OpenText("page.html"))
            {
                return await sr.ReadToEndAsync();
            }
        }

        static async Task<string> GetTransKey()
        {
            var keyUrl = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
            var client = new HttpClient();
            // pass in app Id key
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "22df6e19988c475392dc2418793f81a7");

            
                var resultMessage = await client.PostAsync(keyUrl, new StringContent("") );
                if (resultMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Get hey");
                    return await resultMessage.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine(resultMessage.StatusCode);
                    throw new Exception("Fucked");
                }
            
        }

        static async Task CallTranslateDirect()
        {
            var message = await ReadFile("page.html"); // "This is a message in English that I want translated into another langugage.";
            var from = "en";
            var to = "es";

            var parameters = "text=" + System.Net.WebUtility.UrlEncode($"text={message}") + $"&from={from}&to={to}&contentType=text/html";

            var translateUri = new Uri("https://api.microsofttranslator.com/v2/http.svc/Translate?" + parameters);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "");
            var resultMessage = await client.GetAsync(translateUri);
            Console.WriteLine(resultMessage.StatusCode);
            var Content = await resultMessage.Content.ReadAsStringAsync();

            Console.Write(Content);
        }

        static async Task CallTranslate(string tken)
        {
            var message = await ReadFile("page.html"); // "This is a message in English that I want translated into another langugage.";
            var from = "en";
            var to = "es";

            var parameters = "TextToTranslate=" + System.Net.WebUtility.UrlEncode($"{message}") + $"&from={from}&to={to}&token={tken}";
         //   var parameters = "text=" + System.Net.WebUtility.UrlEncode($"text={message}") + $"&from={from}&to={to}&contentType=text/html";

            var translateUri = new Uri("http://pcltranslate.azurewebsites.net/api/Translator?" + parameters);

            HttpClient client = new HttpClient();
           
            var resultMessage = await client.GetAsync(translateUri);
            Console.WriteLine(resultMessage.StatusCode);
            var Content = await resultMessage.Content.ReadAsStringAsync();

            Console.Write(Content);
        }

        static async Task DoThing()
        {
            var key = await GetTransKey();
          await  CallTranslate(key);
        }


    }
}