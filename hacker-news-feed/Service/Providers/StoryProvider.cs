using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Item;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Providers
{
    public class StoryProvider : IStoryProvider
    {
        private readonly IStoryService _storyService;
        private readonly IStoryCacheService _cache;

        public StoryProvider(IStoryService storyService, IStoryCacheService cache)
        {
            _storyService = storyService;
            _cache = cache;
        }

        public async Task<Item> GetItem(int id)
        {
            var story = _cache.GetStoryCached(id);
            if (story != null)
                return story;

            try
            {
                story = await _storyService.GetItem(id).ConfigureAwait(false);
                if (story == null)
                    return null;

                _cache.AddStoryToCache(story);
                return story;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<int>> GetNewStoryIds()
        {
            var newStories = _cache.GetNewStoryIdsCache();
            if (newStories != null)
                return newStories;

            try
            {
                newStories = await _storyService.GetNewStories().ConfigureAwait(false);
                return _cache.AddNewStoryIdsToCache(newStories);
            }
            catch
            {
                return null;
            }
        }

        public async Task<SortedList<int, Item>> GetNewStories(IEnumerable<int> storyIds)
        {
            var idsNotCached = _cache.GetStoriesNotContained(storyIds);
            IEnumerable<Item> items;
            if(idsNotCached.Any())
            {
                items = await _storyService.GetItems(idsNotCached).ConfigureAwait(false);

                var stories = _cache.AddStoriesToCache(items);
                return stories;
            }
            else
            {
                var stories = _cache.GetStoriesCache();
                if(stories == null)
                {
                    items = await _storyService.GetItems(storyIds).ConfigureAwait(false);
                    stories = _cache.AddStoriesToCache(items);
                }
                return stories;
            }
        }
    }
}
