using hacker_news_feed.Service.Config;
using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Item;
using System;
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
            SortedList<int, Item> stories;
            Item story;
            var foundCachedValue = false;

            var cached = _cache.TryGetValue(CacheKeys.Items, out stories);
            if (!cached)
            {
                stories = new SortedList<int, Item>();
            }
            else if (stories.ContainsKey(id))
            {
                foundCachedValue = true;
                story = stories.GetValueOrDefault(id);
                if (story != null)
                    return story;
            }
            try
            {
                story = await _storyService.GetItem(id).ConfigureAwait(false);
                if (story == null)
                    return null;
            }
            catch
            {
                return null;
            }

            if (foundCachedValue)
                stories[id] = story;
            else
                stories.Add(id, story);
            _cache.Set(CacheKeys.Items, stories);

            return story;
        }

        public async Task<IEnumerable<int>> GetNewStoryIds()
        {
            IEnumerable<int> newStories;

            var cached = _cache.TryGetValue(CacheKeys.NewStories, out newStories);
            if (cached)
                return newStories;

            try
            {
                newStories = await _storyService.GetNewStories().ConfigureAwait(false);
                _cache.Set(CacheKeys.NewStories, newStories);
                return newStories;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<SortedList<int, Item>> GetNewStories(IEnumerable<int> storyIds)
        {
            SortedList<int, Item> stories;
            _ = _cache.TryGetValue(CacheKeys.Items, out stories);

            if (stories == null)
            {
                try
                {
                    var items = await _storyService.GetItems(storyIds).ConfigureAwait(false);
                    if (items == null) return null;

                    stories = new SortedList<int, Item>(items);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                var idsNotCached = storyIds.Where(id => !stories.ContainsKey(id) || stories.GetValueOrDefault(id) == null);
                var items = await _storyService.GetItems(idsNotCached).ConfigureAwait(false);
                foreach (var item in items)
                {
                    var success = stories.TryAdd(item.Key, item.Value);
                    if (!success  && stories.GetValueOrDefault(item.Key) == null)
                    {
                        stories[item.Key] = item.Value;
                    }
                }
            }

            _cache.SetExtendedExpiration(CacheKeys.Items, stories);
            return stories;
        }
    }
}
