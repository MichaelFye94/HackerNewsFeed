using hacker_news_feed.Service.Models.Item;
using System.Collections.Generic;

namespace hacker_news_feed.Service.Interfaces.Story
{
    public interface IStoryCacheService
    {
        Item GetStoryCached(int id);
        IEnumerable<int> GetStoriesNotContained(IEnumerable<int> ids);
        IEnumerable<int> GetNewStoryIdsCache();
        Item AddStoryToCache(Item story);
        SortedList<int, Item> GetStoriesCache();
        IEnumerable<int> AddNewStoryIdsToCache(IEnumerable<int> ids);
        SortedList<int, Item> AddStoriesToCache(IEnumerable<Item> stories);
    }
}
