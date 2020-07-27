using hacker_news_feed.Service.Models.Api;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace hacker_news_feed.Service.Interfaces.Story
{
    public interface IStoryController
    {
        Task<IActionResult> NewStories([FromQuery] ApiQueryModel query);
        Task<IActionResult> Story(int id);
    }
}
