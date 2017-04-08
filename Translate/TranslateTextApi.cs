using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Translate
{
    public class TranslateTextApi
    {
        string Token { get; }
        HttpClient client = new HttpClient();

        public TranslateTextApi(string Token)
        {
            this.Token = Token;
        }

        public async Task<string> Translate(string TextToTranslate, string from, string to)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var parameters = "text=" + System.Net.WebUtility.UrlEncode($"text={TextToTranslate}") + $"&from={from}&to={to}&contentType=text/html";
            var fullUrl = new Uri("https://api.microsofttranslator.com/v2/http.svc/Translator?" + parameters);

            // "Bearer {Token}";
            var resultMessage = await client.GetAsync(fullUrl);
            var Content = await resultMessage.Content.ReadAsStringAsync();
            if (resultMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Content;
            }
            else
            {
                throw new ArgumentOutOfRangeException(Content);
            }
        }

    }
}
