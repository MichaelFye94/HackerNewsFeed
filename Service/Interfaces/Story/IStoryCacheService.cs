using hacker_news_feed.Service.Models.Item;
using System.Collections.Generic;

namespace hacker_news_feed.Service.Interfaces.Story
{
    public interface IStoryCacheService
    {
        bool GetStoryCached(int id, out Item story);
        IEnumerable<int> GetStoriesNotContained(IEnumerable<int> ids);
        bool TryGetNewStoryIdsCache(out IEnumerable<int> newStoryIds);
        Item TryAddStoryToCache(Item story);
        bool TryGetStoriesCache(out SortedList<int, Item> stories);
        IEnumerable<int> TryAddNewStoryIdsToCache(IEnumerable<int> ids);
        SortedList<int, Item> TryAddStoriesToCache(IEnumerable<Item> stories);
    }
}
