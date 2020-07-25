using hacker_news_feed.Service.Models.Item;
using System.Collections.Generic;

namespace hacker_news_feed.Service.Interfaces.Story
{
    public interface IStoryCacheService
    {
        bool GetStoryCached(int id, out Item story);
        IEnumerable<int> GetStoriesNotContained(IEnumerable<int> ids);
        IEnumerable<int> TryGetNewStoryIdsCache();
        Item TryAddStoryToCache(Item story);
        SortedList<int, Item> TryGetStoriesCache();
        IEnumerable<int> TryAddNewStoryIdsToCache(IEnumerable<int> ids);
        SortedList<int, Item> TryAddStoriesToCache(IEnumerable<Item> stories);
    }
}
