using System.Collections.Generic;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Interfaces.Story
{
    public interface IStoryService
    {
        Task<IEnumerable<int>> GetNewStories();
    }
}
