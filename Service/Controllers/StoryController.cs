using hacker_news_feed.Service.Config;
using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Api;
using hacker_news_feed.Service.Models.Item;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Returns a page of new stories
        /// </summary>
        /// <param name="query">Query string parameters with Page (default 1) and PageSize (default 25)</param>
        /// <returns>ApiPagedList of type Item</returns>
        [HttpGet]
        public async Task<IActionResult> NewStories([FromQuery] ApiQueryModel query)
        {
            IEnumerable<int> newStoriesIds;
            if (!_cache.TryGetValue(CacheKeys.NewStories, out newStoriesIds))
            {
                try
                {
                    newStoriesIds = await _storyService.GetNewStories().ConfigureAwait(false);
                }
                catch
                {
                    return BadRequest("Error pulling new stories list");
                }
            }

            var pageList = newStoriesIds.Skip(query.Page - 1).Take(query.PageSize);
            SortedList<int, Item> stories;
            _ = _cache.TryGetValue(CacheKeys.Items, out stories);
            
            if(stories == null)
            {
                var items = await _storyService.GetItems(pageList).ConfigureAwait(false);
                stories = new SortedList<int, Item>(items);
            } else
            {
                var idsNotCached = pageList.Where(id => !stories.ContainsKey(id));
                var items = await _storyService.GetItems(idsNotCached).ConfigureAwait(false);
                foreach(var item in items)
                {
                    stories.TryAdd(item.Key, item.Value);
                }
            }

            if(stories == null)
            {
                return BadRequest("Error pulling new stories");
            }

            var result = pageList.Select(id => stories.GetValueOrDefault(id));

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1)); // Temp val, define in config later
            _cache.Set(CacheKeys.NewStories, newStoriesIds, cacheEntryOptions);
            _cache.Set(CacheKeys.Items, stories, cacheEntryOptions);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Story(int id)
        {
            SortedList<int, Item> stories;
            if (!_cache.TryGetValue(CacheKeys.Items, out stories) || !stories.ContainsKey(id))
            {
                try
                {
                    var story = await _storyService.GetItem(id).ConfigureAwait(false);

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

