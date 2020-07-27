using hacker_news_feed.Service.Models.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Interfaces.Story
{
    public interface IStoryProvider
    {
        Task<Item> GetItem(int id);
        Task<IEnumerable<int>> GetNewStoryIds();
        Task<SortedList<int, Item>> GetNewStories(IEnumerable<int> storyIds);
    }
}
