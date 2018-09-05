using System.Net.Http;
using System.Threading.Tasks;

namespace MiniBook.App.Services
{
    public class HttpService
    {
        public async Task SendAsync(string url, HttpMethod method, HttpContent content = null)
        {
            var request = new HttpRequestMessage(method, url);
            if (content != null)
                request.Content = content;

            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);            
                
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    // Read body as Json
                }
            }

        }

        private HttpClient DefaultHttpClient()
        {
            return new HttpClient();
        }
    }
}
