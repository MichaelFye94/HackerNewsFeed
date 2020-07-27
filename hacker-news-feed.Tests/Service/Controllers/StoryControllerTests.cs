using hacker_news_feed.Controllers;
using hacker_news_feed.Service.Interfaces.Story;
using hacker_news_feed.Service.Models.Api;
using hacker_news_feed.Service.Models.Item;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hacker_news_feed.Tests.Service.Controllers
{
    [TestFixture]
    class StoryControllerTests
    {
        private IStoryController _storyController;

        public void SetUp()
        {
            var mockStoryProvider = new Mock<IStoryProvider>();

            IEnumerable<int> pageList = new List<int> { 1, 2 };
            mockStoryProvider.Setup(x => x.GetNewStoryIds())
                .Returns<IEnumerable<int>>(null);
            mockStoryProvider.Setup(x => x.GetNewStories(pageList))
                .Returns<IEnumerable<int>>(null);
        }

        [Test]
        public async Task NewStories_NullNewStoriesEds_ReturnsBadRequest()
        {
            var mockStoryProvider = new Mock<IStoryProvider>();

            mockStoryProvider.Setup(x => x.GetNewStoryIds())
                .Returns(Task.FromResult<IEnumerable<int>>(null));

            _storyController = new StoryController(mockStoryProvider.Object);
            var query = new ApiQueryModel
            {
                Page = 1,
                PageSize = 2
            };
            var result = await _storyController.NewStories(query);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task NewStories_NullNewStories_ReturnsBadRequest()
        {
            var mockStoryProvider = new Mock<IStoryProvider>();

            IEnumerable<int> pageList = new List<int> { 1, 2 };
            mockStoryProvider.Setup(x => x.GetNewStoryIds())
                .Returns(Task.FromResult(pageList));
            mockStoryProvider.Setup(x => x.GetNewStories(pageList))
                .Returns(Task.FromResult<SortedList<int, Item>>(null));

            _storyController = new StoryController(mockStoryProvider.Object);
            var query = new ApiQueryModel
            {
                Page = 1,
                PageSize = 2
            };
            var result = await _storyController.NewStories(query);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task NewStories_PageOutOfBounds_ReturnsBadRequest()
        {
            var mockStoryProvider = new Mock<IStoryProvider>();

            IEnumerable<int> pageList = new List<int> { 1, 2 };
            mockStoryProvider.Setup(x => x.GetNewStoryIds())
                .Returns(Task.FromResult(pageList));

            _storyController = new StoryController(mockStoryProvider.Object);
            var query = new ApiQueryModel
            {
                Page = 3,
                PageSize = 2
            };
            var result = await _storyController.NewStories(query);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task NewStories_NotNullNewStories_ReturnsOk()
        {
            var mockStoryProvider = new Mock<IStoryProvider>();

            IEnumerable<int> pageList = new List<int> { 1, 2 };
            var story_1 = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };

            var story_2 = new Item
            {
                Id = 2,
                Title = "Test 2",
                Type = ItemType.Story,
                Url = "https://stackoverflow.com"
            };
            var stories = new SortedList<int, Item>();
            stories.Add(story_1.Id, story_1);
            stories.Add(story_2.Id, story_2);

            mockStoryProvider.Setup(x => x.GetNewStoryIds())
                .Returns(Task.FromResult(pageList));
            mockStoryProvider.Setup(x => x.GetNewStories(pageList))
                .Returns(Task.FromResult(stories));

            _storyController = new StoryController(mockStoryProvider.Object);

            var query = new ApiQueryModel
            {
                Page = 1,
                PageSize = 2
            };
            var result = await _storyController.NewStories(query);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task Story_NullStory_ReturnsBadRequest()
        {
            var mockStoryProvider = new Mock<IStoryProvider>();

            mockStoryProvider.Setup(x => x.GetItem(It.IsAny<int>()))
                .Returns(Task.FromResult<Item>(null));

            _storyController = new StoryController(mockStoryProvider.Object);

            var result = await _storyController.Story(1);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Story_ValidStory_ReturnsOk()
        {
            var mockStoryProvider = new Mock<IStoryProvider>();

            var story = new Item
            {
                Id = 1,
                Title = "Test",
                Type = ItemType.Story,
                Url = "https://www.google.com"
            };
            mockStoryProvider.Setup(x => x.GetItem(1))
                .Returns(Task.FromResult(story));

            _storyController = new StoryController(mockStoryProvider.Object);

            var result = await _storyController.Story(1);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }
    }
}
