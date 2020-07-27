using hacker_news_feed.Service.Abstract.Data;
using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Item;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Data
{
    public class StoryData : ApiRequestBase, IStoryData
    {
        public StoryData(HttpClient client) : base(client) { }

        public async Task<IEnumerable<int>> GetNewStories()
        {
            var url = "newstories";
            return await GetAsync<IEnumerable<int>>(url).ConfigureAwait(false);
        }

        public async Task<Item> GetStory(int id)
        {
            var url = $"item/{id}";
            return await GetAsync<Item>(url).ConfigureAwait(false);
        }
    }
}
