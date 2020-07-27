using hacker_news_feed.Service.Config;
using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Item;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hacker_news_feed.Service.Services
{
    public class StoryCacheService : IStoryCacheService
    {
        private IMemoryCache _cache;
        private MemoryCacheEntryOptions _defaultOptions, _extendedExpirationOptions;

        public StoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
            _defaultOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1));
            _extendedExpirationOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));
        }

        public Item GetStoryCached(int id)
        {
            var storyCache = GetStoriesCache();
            if (storyCache != null)
            {
                return storyCache.GetValueOrDefault(id);
            }
            return null;
        }

        public IEnumerable<int> GetStoriesNotContained(IEnumerable<int> ids)
        {
            var storyCache = GetStoriesCache();
            if (storyCache == null)
                return ids;

            return ids.Where(id => !storyCache.ContainsKey(id) || storyCache[id] == null);
        }

        public IEnumerable<int> GetNewStoryIdsCache()
        {
            IEnumerable<int> newStoryIds;
            _cache.TryGetValue(CacheKeys.NewStories, out newStoryIds);
            return newStoryIds;
        }

        public SortedList<int, Item> GetStoriesCache()
        {
            SortedList<int, Item> stories;
            _cache.TryGetValue(CacheKeys.Items, out stories);
            return stories;
        }

        public IEnumerable<int> AddNewStoryIdsToCache(IEnumerable<int> ids)
            => SetNewStoryIdsCache(ids);

        public Item AddStoryToCache(Item story)
        {
            if (story == null)
                return null;

            var storyCache = GetStoriesCache();
            if (storyCache == null)
                storyCache = new SortedList<int, Item>();

            var success = storyCache.TryAdd(story.Id, story);
            if (!success && storyCache.GetValueOrDefault(story.Id) == null)
                storyCache[story.Id] = story;

            SetStoriesCache(storyCache);
            return story;
        }

        public SortedList<int, Item> AddStoriesToCache(IEnumerable<Item> stories)
        {
            if (stories == null)
                return null;

            var storyCache = GetStoriesCache();
            if (storyCache == null)
                storyCache = new SortedList<int, Item>();

            foreach (var story in stories)
            {
                var success = storyCache.TryAdd(story.Id, story);
                if (!success && storyCache.GetValueOrDefault(story.Id) == null)
                    storyCache[story.Id] = story;
            }

            return SetStoriesCache(storyCache);
        }

        private T Set<T>(object key, T value)
            => _cache.Set(key, value, _defaultOptions);

        private T SetExtendedExpiration<T>(object key, T value)
            => _cache.Set(key, value, _extendedExpirationOptions);

        private IEnumerable<int> SetNewStoryIdsCache(IEnumerable<int> newStoryIds)
            => Set(CacheKeys.NewStories, newStoryIds);

        private SortedList<int, Item> SetStoriesCache(SortedList<int, Item> items)
            => SetExtendedExpiration(CacheKeys.Items, items);

    }
}