using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Api;
using hacker_news_feed.Service.Models.Item;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hacker_news_feed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase, IStoryController
    {
        private readonly IStoryProvider _storyProvider;

        public StoryController(IStoryProvider storyProvider)
        {
            _storyProvider = storyProvider;
        }

        /// <summary>
        /// Returns a page of new stories
        /// </summary>
        /// <param name="query">Query string parameters with Page (default 1) and PageSize (default 25)</param>
        /// <returns>ApiPagedList of type Item</returns>
        [HttpGet]
        public async Task<IActionResult> NewStories([FromQuery] ApiQueryModel query)
        {
            var newStoryIds = await _storyProvider.GetNewStoryIds();
            if(newStoryIds == null)
            {
                return BadRequest("Error loading new story list");
            }

            var pageOffset = (query.Page - 1) * query.PageSize;
            if(pageOffset >= newStoryIds.Count())
            {
                return BadRequest("Invalid requested range");
            }

            var pageList = newStoryIds.Skip(pageOffset).Take(query.PageSize);
            var stories = await _storyProvider.GetNewStories(pageList);
            if(stories == null)
            {
                return BadRequest("Error pulling new stories");
            }

            var paginatedResult = pageList.Select(id => stories.GetValueOrDefault(id));
            var currentPage = query.Page;
            var lastPage = (int)Math.Ceiling((double)newStoryIds.Count() / query.PageSize);

            var result = new ApiPagedList<Item>(
                paginatedResult,
                currentPage,
                lastPage
            );

            return Ok(result);
        }

        /// <summary>
        /// Returns a specific story item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Story(int id)
        {
            var story = await _storyProvider.GetItem(id);
            if (story == null)
                return BadRequest("Error loading story");
            return Ok(story);
        }
    }
}

