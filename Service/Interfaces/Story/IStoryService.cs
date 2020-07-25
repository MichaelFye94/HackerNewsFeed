using hacker_news_feed.Service.Models.Item;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Interfaces.Story
{
    public interface IStoryService
    {
        Task<IEnumerable<int>> GetNewStories();
        Task<Item> GetItem(int id);
        Task<IEnumerable<Item>> GetItems(IEnumerable<int> ids);
    }
}
