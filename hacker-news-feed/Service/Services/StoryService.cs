using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Item;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryData _storyData;

        public StoryService(IStoryData storyData)
        {
            _storyData = storyData;
        }

        public async Task<IEnumerable<int>> GetNewStories()
        {
            return await _storyData.GetNewStories();
        }

        public async Task<Item> GetItem(int id)
        {
            return await _storyData.GetStory(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Item>> GetItems(IEnumerable<int> ids)
        {
            var stories = new ConcurrentBag<Item>();
            foreach (var id in ids)
            {
                var item = await GetItem(id).ConfigureAwait(false);
                if(item != null)
                    stories.Add(item);
            }
            return stories;
        }
    }
}