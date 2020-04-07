using MiniBook.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MiniBook.Services
{
    public class HttpService
    {
        public async Task<T> SendAsync<T>(string url, HttpMethod method, HttpContent content = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (content != null)
                request.Content = content;

            if (AppContext.Current.Token?.IsExpired() == false)
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", AppContext.Current.Token.AccessToken);
            }

            using (var client = DefaultHttpClient())
            {
                var response = await client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(body);
            }
        }

        internal Task<ApiResponse<T>> PostApiAsync<T>(string url, object body)
        {
            return PostAsync<ApiResponse<T>>(url, body);
        }

        internal Task<T> PostAsync<T>(string url, object body)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            return SendAsync<T>(url, HttpMethod.Post, content);
        }

        internal Task<T> PostAsync<T>(string url, Dictionary<string, string> form)
        {
            var content = new FormUrlEncodedContent(form);

            return SendAsync<T>(url, HttpMethod.Post, content);
        }

        private HttpClient DefaultHttpClient()
        {
            return new HttpClient();
        }

        public Task<T> GetAsync<T>(string url)
        {
            return SendAsync<T>(url, HttpMethod.Get);
        }
    }
}
