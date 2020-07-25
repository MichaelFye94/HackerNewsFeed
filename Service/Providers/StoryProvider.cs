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
            Item story;

            var cached = _cache.GetStoryCached(id, out story);
            if (cached)
                return story;

            try
            {
                story = await _storyService.GetItem(id).ConfigureAwait(false);
                if (story == null)
                    return null;

                _cache.TryAddStoryToCache(story);
                return story;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<int>> GetNewStoryIds()
        {
            IEnumerable<int> newStories;

            var cached = _cache.TryGetNewStoryIdsCache(out newStories);
            if (cached)
                return newStories;

            try
            {
                newStories = await _storyService.GetNewStories().ConfigureAwait(false);
                return _cache.TryAddNewStoryIdsToCache(newStories);
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

                var stories = _cache.TryAddStoriesToCache(items);
                return stories;
            }
            else
            {
                SortedList<int, Item> stories;
                _ = _cache.TryGetStoriesCache(out stories);
                if(stories == null)
                {
                    items = await _storyService.GetItems(storyIds).ConfigureAwait(false);
                    stories = _cache.TryAddStoriesToCache(items);
                }
                return stories;
            }
        }
    }
}
