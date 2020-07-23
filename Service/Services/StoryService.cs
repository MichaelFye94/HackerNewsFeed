using hacker_news_feed.Service.Abstract.Services;
using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Item;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Services
{
    public class StoryService : BaseService, IStoryService
    {
        private readonly IStoryData _storyData;
        public StoryService(HttpClient httpClient, IStoryData storyData) : base(httpClient)
        {
            _storyData = storyData;
        }

        public async Task<IEnumerable<int>> GetNewStories()
        {
            return await _storyData.GetNewStories();
        }

        public async Task<Item> GetStory(int id)
        {
            return await _storyData.GetStory(id);
        }
    }
}
