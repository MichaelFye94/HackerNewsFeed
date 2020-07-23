using hacker_news_feed.Service.Config;
using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Api;
using hacker_news_feed.Service.Models.Item;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> NewStories([FromQuery] ApiQueryModel query)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Story(int id)
        {
            SortedList<int, Item> stories;
            if (!_cache.TryGetValue(CacheKeys.Items, out stories) || !stories.ContainsKey(id))
            {
                try
                {
                    var story = await _storyService.GetStory(id);

                    if(stories == null)
                    {
                        stories = new SortedList<int, Item>();
                    }
                    stories.Add(id, story);
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1)); // Temp val, define in config later
                    _cache.Set(CacheKeys.Items, stories, cacheEntryOptions);

                    return Ok(story);
                }
                catch (Exception)
                {
                    return BadRequest($"Error pulling story {id}");
                }
            }

            return Ok(stories.GetValueOrDefault(id));
        }
    }
}
