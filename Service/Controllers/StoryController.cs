using hacker_news_feed.Service.Config;
using hacker_news_feed.Service.Interfaces.Story;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hacker_news_feed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly ILogger<StoryController> _logger;
        private IMemoryCache _cache;

        public StoryController(IStoryService storyService, ILogger<StoryController> logger, IMemoryCache cache)
        {
            _storyService = storyService;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<int> newStories;

            // Check cache for new stories
            if (!_cache.TryGetValue(CacheKeys.NewStories, out newStories))
            {
                try
                {
                    newStories = await _storyService.GetNewStories();

                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1)); // Temp val, define in config later
                    _cache.Set(CacheKeys.NewStories, newStories, cacheEntryOptions);

                    return Ok(newStories);
                }
                catch
                {
                    return BadRequest("Error pulling new stories");
                }
            }

            return Ok(newStories);
        }
    }
}
