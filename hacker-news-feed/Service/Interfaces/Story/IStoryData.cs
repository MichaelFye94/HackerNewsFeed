using hacker_news_feed.Service.Models.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Interfaces.Story
{
    public interface IStoryData
    {
        Task<Item> GetStory(int id);
        Task<IEnumerable<int>> GetNewStories();
    }
}
