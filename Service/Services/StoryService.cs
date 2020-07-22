using hacker_news_feed.Service.Abstract.Services;
using hacker_news_feed.Service.Interfaces.Story;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Services
{
    public class StoryService : BaseService, IStoryService
    {
        public StoryService(HttpClient httpClient) : base(httpClient)
        {

        }

        public async Task<IEnumerable<int>> GetNewStories()
        {
            var url = "newstories";
            return await GetAsync<IEnumerable<int>>(url);
        }
    }
}
