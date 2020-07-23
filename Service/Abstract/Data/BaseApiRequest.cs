using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Abstract.Data
{
    public abstract class BaseApiRequest
    {
        private HttpClient _httpClient;
        public BaseApiRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        protected async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(BuildJsonUrl(url));
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                // Parse response from responseBody
                var result = JsonConvert.DeserializeObject<T>(responseBody);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        protected string BuildJsonUrl(string url) => $"{url}.json";
    }
}
